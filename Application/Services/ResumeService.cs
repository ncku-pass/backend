using Application.Dto.Messages;
using Application.Dto.Responses;
using Application.Services.Interface;
using AutoMapper;
using Infrastructure.Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IExperienceService _experienceService;
        private readonly int _userId;

        public ResumeService(
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IExperienceService experienceService
            )
        {
            this._httpContextAccessor = httpContextAccessor;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._experienceService = experienceService;
            try
            {
                this._userId = int.Parse(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            catch
            {
                this._userId = 1;
            }
        }

        /// <summary>
        /// 取得全部履歷
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<ResumeResponse>> GetResumesAsync()
        {
            var resumeModels = await _unitOfWork.Resume.Where(e => e.UserId == this._userId).ToListAsync();
            var cardModels = await this._unitOfWork.Card.Where(e => e.UserId == this._userId).ToListAsync();
            var cardIds = cardModels.Select(t => t.Id).ToList();
            var card_ExpModels = await _unitOfWork.Card_Experience.Where(ce => cardIds.Contains(ce.CardId)).ToListAsync();

            var resumeResponses = _mapper.Map<List<ResumeResponse>>(resumeModels);
            var cardResponses = _mapper.Map<List<CardResponse>>(cardModels);
            var expResponses = await _experienceService.GetExperiencesAsync();

            // 填入每張card的expInCard
            foreach (var card in cardResponses)
            {
                var expInCardList = new List<ExpInCardResponse>() { };
                var card_expModels = card_ExpModels.Where(ce => ce.CardId == card.Id).ToList();
                foreach (var card_exp in card_expModels)
                {
                    var expInCardResponse = new ExpInCardResponse() { };
                    var exp = expResponses.SingleOrDefault(e => e.Id == card_exp.ExperienceId);
                    this._mapper.Map(card_exp, expInCardResponse);
                    this._mapper.Map(exp, expInCardResponse);
                    expInCardList.Add(expInCardResponse);
                }
                card.Experiences = expInCardList.OrderBy(e => e.Order).ToList();
            }

            foreach (var resume in resumeResponses)
            {
                resume.Cards = cardResponses.Where(c => c.ResumeId == resume.Id).OrderBy(c => c.Order).ToList();
            }

            return resumeResponses;
        }

        /// <summary>
        /// 依Id取得履歷
        /// </summary>
        /// <param name="resumeId"></param>
        /// <returns></returns>
        public async Task<ResumeResponse> GetResumeByIdAsync(int resumeId)
        {
            // 撈出資料庫對應Model
            var resumeModel = await _unitOfWork.Resume.SingleOrDefaultAsync(e => e.UserId == this._userId && e.Id == resumeId);
            var cardModels = await this._unitOfWork.Card.Where(e => e.ResumeId == resumeId).ToListAsync();
            var cardIds = cardModels.Select(t => t.Id).ToList();
            var card_ExpModels = await _unitOfWork.Card_Experience.Where(te => cardIds.Contains(te.CardId)).ToListAsync();

            var resumeResponse = _mapper.Map<ResumeResponse>(resumeModel);
            var cardResponses = _mapper.Map<List<CardResponse>>(cardModels);
            var expResponses = await _experienceService.GetExperiencesAsync();

            // 撈card中的exp存入card.experience
            foreach (var card in cardResponses)
            {
                var expInCardList = new List<ExpInCardResponse>() { };
                var card_expModels = card_ExpModels.Where(te => te.CardId == card.Id).ToList();
                foreach (var card_exp in card_expModels)
                {
                    var expInCardResponse = new ExpInCardResponse() { };
                    var exp = expResponses.SingleOrDefault(e => e.Id == card_exp.ExperienceId);
                    this._mapper.Map(card_exp, expInCardResponse);
                    this._mapper.Map(exp, expInCardResponse);
                    expInCardList.Add(expInCardResponse);
                }
                card.Experiences = expInCardList.OrderBy(e => e.Order).ToList();
            }
            resumeResponse.Cards = cardResponses.OrderBy(c => c.Order).ToList();

            return resumeResponse;
        }

        /// <summary>
        /// 儲存履歷
        /// </summary>
        /// <param name="resumeSaveMessage"></param>
        /// <returns></returns>
        public async Task<ResumeResponse> SaveResumeAsync(ResumeSaveMessage resumeSaveMessage)
        {
            // 依排序給值card和expInCard的order
            resumeSaveMessage.InitOrder();

            // 建立or更新資料庫的Resume
            var resumeModel = _mapper.Map<Resume>(resumeSaveMessage);
            resumeModel.UserId = this._userId;
            if (resumeModel.Id == 0)//// 新Resume
            {
                this._unitOfWork.Resume.Add(resumeModel);
            }
            else
            {
                this._unitOfWork.Resume.Update(resumeModel);
            }
            //this._unitOfWork.SaveChange();
            await this._unitOfWork.SaveChangeAsync();

            // 建立or更新資料庫的Card
            var cardModelList = new List<Card>();
            foreach (var card in resumeSaveMessage.Cards)
            {
                var cardModel = this._mapper.Map<Card>(card);
                cardModel.UserId = this._userId;
                cardModel.ResumeId = resumeModel.Id;
                if (cardModel.Id == 0)
                {
                    this._unitOfWork.Card.Add(cardModel);
                }
                else
                {
                    this._unitOfWork.Card.Update(cardModel);
                }
                cardModelList.Add(cardModel);
            }
            await this._unitOfWork.SaveChangeAsync();

            // 從剛剛儲存的Model映射CardId到Message
            using (var message = resumeSaveMessage.Cards.GetEnumerator())
            using (var model = cardModelList.GetEnumerator())
            {
                while (message.MoveNext() && model.MoveNext())
                {
                    message.Current.Id = model.Current.Id;
                }
            }

            // 新增&更新Card_Exp關聯
            foreach (var card in resumeSaveMessage.Cards)
            {
                if (card.Type == "text")
                {
                    continue;
                }

                // 用字典存入現有的card_exp：Key放ExpId，Value放model
                Dictionary<int, Card_Experience> card_ExpDictionary = new Dictionary<int, Card_Experience>();
                foreach (var card_exp in await this._unitOfWork.Card_Experience.Where(te => te.CardId == card.Id).ToListAsync())
                {
                    card_ExpDictionary.Add(card_exp.ExperienceId, card_exp);
                }

                foreach (var card_expMessage in card.Experiences)
                {
                    // 從字典檢查是否是現有card_exp
                    if (card_ExpDictionary.ContainsKey(card_expMessage.Id))
                    {
                        // 有則映射card_exp上去
                        var card_expModel = card_ExpDictionary[card_expMessage.Id];
                        card_expModel.Order = card_expMessage.Order;
                        card_expModel.ShowFeedback = card_expMessage.ShowFeedback;
                        card_expModel.ShowPosition = card_expMessage.ShowPosition;
                        this._unitOfWork.Card_Experience.Update(card_expModel);
                    }
                    else
                    {
                        var card_expModel = new Card_Experience()
                        {
                            CardId = card.Id,
                            ExperienceId = card_expMessage.Id,
                            Order = card_expMessage.Order,
                            ShowFeedback = card_expMessage.ShowFeedback,
                            ShowPosition = card_expMessage.ShowPosition
                        };
                        this._unitOfWork.Card_Experience.Add(card_expModel);
                    }
                }
            }
            await this._unitOfWork.SaveChangeAsync();

            // 刪除指定Card_Exp關聯
            foreach (var card in resumeSaveMessage.Cards)
            {
                if (card.Type == "text")
                {
                    continue;
                }
                var currentCard_ExpModels = await this._unitOfWork.Card_Experience.Where(te => te.CardId == card.Id).ToListAsync();
                foreach (var card_exp in currentCard_ExpModels)
                {
                    if (card.DeleteExpIds.Contains(card_exp.ExperienceId))
                    {
                        this._unitOfWork.Card_Experience.Remove(card_exp);
                    }
                }
            }
            await this._unitOfWork.SaveChangeAsync();

            // 刪除Card
            foreach (var id in resumeSaveMessage.DeleteCardIds)
            {
                await this.DeleteCardAsync(resumeSaveMessage.Id, id);
            }

            return _ = await GetResumeByIdAsync(resumeModel.Id);
        }

        /// <summary>
        /// 依Id查詢履歷是否存在
        /// </summary>
        /// <param name="resumeId"></param>
        /// <returns></returns>
        public async Task<bool> ResumeExistsAsync(int resumeId)
        {
            return await this._unitOfWork.Resume.AnyAsync(e => e.UserId == this._userId && e.Id == resumeId);
        }

        /// <summary>
        /// 移除Resume
        /// </summary>
        /// <param name="resumeId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteResumeAsync(int resumeId)
        {
            var resumeModel = await this._unitOfWork.Resume.SingleOrDefaultAsync(t => t.Id == resumeId && t.UserId == this._userId);
            var cardModels = await this._unitOfWork.Card.Where(t => t.UserId == this._userId && t.ResumeId == resumeId).ToListAsync();

            // 移除該Resume全部的Card以及其card_exp關聯
            foreach (var item in cardModels)
            {
                await this.DeleteCardAsync(resumeId, item.Id);
            }

            // 移除Exp
            this._unitOfWork.Resume.Remove(resumeModel);
            return await this._unitOfWork.SaveChangeAsync();
        }

        /// <summary>
        /// 移除Card
        /// </summary>
        /// <param name="resumeId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCardAsync(int resumeId, int cardId)
        {
            if (!await this.CardExistsAsync(resumeId, cardId))
            {
                return await this._unitOfWork.SaveChangeAsync();
            }
            var cardModel = await this._unitOfWork.Card.SingleOrDefaultAsync(t => t.Id == cardId && t.UserId == this._userId && t.ResumeId == resumeId);

            // 移除該Card全部的card_exp關聯
            var card_ExpModels = await this._unitOfWork.Card_Experience.Where(n => n.CardId == cardId).ToListAsync();
            this._unitOfWork.Card_Experience.RemoveRange(card_ExpModels);

            // 移除Exp
            this._unitOfWork.Card.Remove(cardModel);
            return await this._unitOfWork.SaveChangeAsync();
        }

        /// <summary>
        /// 依Id查詢主題是否存在
        /// </summary>
        /// <param name="resumeId"></param>
        /// <param name="cardId"></param>
        /// <returns></returns>
        public async Task<bool> CardExistsAsync(int resumeId, int cardId)
        {
            return await this._unitOfWork.Card.AnyAsync(e => e.UserId == this._userId && e.Id == cardId && e.ResumeId == resumeId);
        }
    }
}