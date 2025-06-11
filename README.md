# Project14 - 進銷存管理系統

這是一個使用 ASP.NET MVC 開發的進銷存管理系統，支援會員註冊、商品瀏覽、購物車、訂單管理、後台控管等功能，適合作為教學專案或中小企業資訊化改造範例。

## 🛠️ 技術架構

- ASP.NET MVC 5
- Entity Framework 6 (Database First)
- SQL Server / Azure SQL
- Bootstrap 4 + jQuery
- C# (.NET Framework)

## 📦 功能模組

### 前台功能（會員）

- 📝 會員註冊 / 登入 / 登出
- 🛒 加入購物車 / 編輯數量 / 移除商品
- ✅ 訂單送出 / 即時付款紀錄
- 📬 查詢歷史訂單與明細

### 後台功能（管理員）

- 🔐 後台登入
- 📦 商品管理（新增 / 編輯 / 刪除）
- 🗂️ 商品分類管理
- 📑 訂單 / 付款 / 出貨紀錄查詢
- 👥 會員資料檢視

## ⚙️ 資料表架構（簡要）

- `table_Member`：會員資料
- `table_Product`：商品資訊
- `table_Category`：商品分類
- `table_ShoppingCart`：購物車
- `table_Order`：訂單主檔
- `table_OrderDetail`：訂單明細
- `table_PaymentLog`：付款紀錄
- `table_ShipmentLog`：出貨紀錄
- `table_Admin`：後台帳號


