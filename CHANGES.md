# Changes Summary

## Backend (Core/OpenCRM.Core.Web)
- Added `Controllers/AuthController.cs` with login, register, and email confirmation endpoints using IdentityService and email sending.
- Introduced auth DTOs for requests/responses under `Models/Auth` (login, register, confirm email).
- Enabled controller mapping in `Web/Program.cs`.

## Frontend (Client Angular)
- Added generic `ApiService` and auth-specific `AuthApiService`.
- Created auth DTO interfaces under `Client/src/app/models/auth`.
- Implemented global `AuthStore` for logged user state.
- Updated login page to call backend auth APIs and reflect status.
- Added registration page and route for user signup.
