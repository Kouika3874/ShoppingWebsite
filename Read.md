# 🛒 PChouse 24H 購物系統

這是一個以 ASP.NET MVC 5 開發的購物網站系統，具備完整的會員系統、商品瀏覽、加入購物車、訂單管理功能。系統導入 ViewModel 分層架構，並加入 Ajax 與 SHA256 加密，提升安全性與使用者體驗。

---

## 📦 系統功能簡介

### 👤 會員系統

- 註冊 / 登入 / 登出（含驗證碼與記住我）
- 個人資料檢視與修改密碼（密碼 SHA256 加密，強度檢查）

### 🛒 商品與購物車

- 商品首頁瀏覽（含圖片、單價）
- 加入購物車（支援 Ajax 無刷新）
- 購物車清單與刪除功能
- 填寫收件人資訊並送出訂單

### 📑 訂單管理

- 訂單總覽（日期、狀態）
- 單筆訂單明細（產品清單與數量）

---

## 🔧 使用技術

- ASP.NET MVC 5 + Razor View Engine
- Entity Framework 6（使用 LocalDB 資料庫）
- ViewModel 架構分層
- Bootstrap 5 + jQuery
- 密碼加密：SHA256
- 驗證碼產生：圖像驗證碼（後端動態繪圖）
- Ajax 加入購物車（JSON 回傳）

---

## 🗂 專案資料夾結構

```
Project14/
├── Controllers/
│   ├── HomeController.cs
│   └── MemberController.cs
├── Models/
│   └── table_*.cs
├── ViewModels/
│   └── *.cs（各頁 ViewModel）
├── Views/
│   ├── Home/
│   ├── Member/
│   └── Shared/
├── Helpers/
│   └── PasswordHelper.cs
├── Scripts/
│   └── custom.js（自訂互動）
├── Content/
│   └── site.css（自訂樣式）
├── App_Data/
│   └── dbShoppingCar.mdf
└── README.md
```

