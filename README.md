# NCKU PASS ｜ 線上學習經歷整理平台
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)

這是一個由成大學生開發的學習經歷整理幫手，我們希望能幫助學生
- :star: 紀錄大學各項經驗 
- :star: 以TAG整理個人能力 
- :star: 主題式歸納經歷

「 NCKU PASS 陪你整理學習經歷，全方位展現個人特質 :rainbow: ️」

---
```
Backend
│
├─Api                           // API層
│  ├─Controllers                    // Controllers：API入口
│  ├─Profiles                       // Profiles：Automapper各層Model對照註冊
│  └─RequestModel                   // RequestModel：API層Model
│      ├─Parameters                     // Parameter：前端傳入參數
│      │  └─Validations                     // Validations：Parameter參數驗證
│      └─ViewModels                     // ViewModel：回傳前端參數
│
├─Application                   // Service層
│  ├─Dto                            // Dto：Service層Model
│  │  ├─Messages                        // Message：API層傳入參數
│  │  └─Responses                       // Response：回傳API層參數
│  └─Services                       // Services：後端程式商業邏輯
│
└─Infrastructure                // Repository層
    ├─Infrastructure                // Infra：資料庫CRUD程式碼
    ├─Migrations                    // Migration：DB映射文件資料夾
    ├─Models                        // Models：DB Table原始類別
    │  └─Enums                          // Enums：列舉
    └─Services                      // Services：外部服務或輔助工具(成功入口api、檔案存讀取、AES加解密工具)
```
---

#### :speech_balloon: Find Us  
[![Instagram](https://img.shields.io/badge/Instagram-%23E4405F.svg?style=flat-squar&logo=Instagram&logoColor=white)](https://instagram.com/ncku.pass?igshid=YmMyMTA2M2Y=)
[![Facebook](https://img.shields.io/badge/Facebook-%231877F2.svg?style=flat-squar&logo=Facebook&logoColor=white)](https://www.facebook.com/nckupass)