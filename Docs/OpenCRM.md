# OpenCRM

The  open source cyphered content resources manager.

## General Architecture

![Diagram](./Files/OpenCRM_Architecture_Diagram.png)

## Notes

### OpenCRM.Web (Modules/OpenCRM.Manager) is a not needed head project

- The OpenCRM.Core.Web most contain all UI needed parts to be used on and extertal ASP Net Core Web project
- The CoreLayout most be  available rto be used on any Core Razor Page
- For the core web pages and components use bootstrap components as default styles

## Important for page logic implementations

- Please use separation of concerns. Use services to implement pages business logic
