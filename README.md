📝 **TodoApp - Hướng Dẫn Chạy Dự Án**

> ⭐ _Một ứng dụng quản lý công việc đơn giản, sử dụng Middlewares

---

## 📌 Mục lục
1. [Chuẩn bị](#-chuẩn-bị)
2. [Cài đặt](#-cài-đặt)
3. [Thiết lập cơ sở dữ liệu](#-thiết-lập-cơ-sở-dữ-liệu)
4. [Mở dự án trong Visual Studio](#-mở-dự-án-trong-visual-studio)
5. [Cài đặt Package](#-cài-đặt-package)
6. [Chạy ứng dụng](#-chạy-ứng-dụng)

---

## 📦 1. Chuẩn bị
- **.NET SDK** 8.0
- **SQL Server** (bản Community hoặc cao hơn)
- **Visual Studio 2022** hoặc mới hơn

---

## 🔄 2. Clone dự án
```bash
# Phương án 1: Clone từ GitHub
git clone https://github.com/TranDaoBaThanh/CongNgheNET_Giuaki.git

# Phương án 2: Tải file ZIP và giải nén
```

---

## 💾 3. Thiết lập cơ sở dữ liệu
1. Mở **SQL Server Management Studio** (SSMS).
2. Kết nối tới server mong muốn.
3. Chạy file `TodoApp.sql` để tạo schema & bảng:
```sql
-- Mở file TodoApp.sql và nhấn Execute
```

---

## 🖥️ 4. Mở dự án trong Visual Studio
1. Chạy Visual Studio.
2. Chọn **Get Started** → **Open a project or solution**.
3. Dẫn đến thư mục chứa mã nguồn, chọn `TodoApp.csproj` và nhấn **Open**.

---

## 📥 5. Cài đặt Package
Mở **Package Manager Console** (Tools → NuGet Package Manager → Package Manager Console) và chạy:
```powershell
dotnet add package Microsoft.EntityFrameworkCore

dotnet add package Microsoft.EntityFrameworkCore.SqlServer

dotnet add package Microsoft.EntityFrameworkCore.Design

dotnet add package Microsoft.EntityFrameworkCore.Tools
```

---

## ▶️ 6. Chạy ứng dụng
1. Chọn vào nút có mũi tên màu xanh và chữ TodoApp (nếu không chạy được có thể chạy lệnh như sau : dotnet restore -> dotnet build -> dotnet run để chạy code)
2. Trình duyệt tự động mở, có thể bắt đầu đăng nhập/đăng ký tài khoản, thêm, sửa, xoá công việc.

---

❤️ _Happy Coding!_  

