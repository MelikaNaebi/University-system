# راهنمای اجرای پروژه سامانه دانشگاهی

این فایل حاوی مراحل و نکات لازم برای راه‌اندازی و اجرای پروژه در محیط محلی (Local) است.

## پیش‌نیازها
قبل از شروع، مطمئن شوید ابزارهای زیر روی سیستم شما نصب باشند:
*   .NET SDK (نسخه 8 یا بالاتر)
*   SQL Server Management Studio (SSMS)

---

## مراحل راه‌اندازی و اجرا

### ۱. تنظیم رشته اتصال (Connection String)
1. وارد پوشه پروژه بک‌اَند (`Backend`) شوید.
2. فایل `appsettings.json` را باز کنید.
3. در بخش `ConnectionStrings`، مقدار دیتابیس را متناسب با سرور SQL خود تغییر دهید:
   ```json
   "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=UniversityDb;Trusted_Connection=True;TrustServerCertificate=True;"