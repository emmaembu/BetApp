Betting Application
This repository contains a  betting application built with C#, .NET, Entity Framework Core and follows Clean Architecture principles.

Overview
This application allows users to place bets on sports matches, manage wallets and process transactions. It separates concerns between the Domain, Application and Infrastructure layers.

Architecture
Domain Layer: Contains entities and business rules. All domain logic (calculating payout, fee, top offer rules) is encapsulated here.
Application Layer: Contains services, validators, DTOs, mapper, interfaces.
Infrastructure Layer: Contains EF Core repositories, database configurations,mapper, seeds.

Features
- Place bets using BetSlip and BetItem.
- Validate bets using FluentValidation.
- Wallet management with deposits and transaction history.
- Top Offer and market-specific rules.
- Outbox pattern for reliable event processing.
- Async worker service for transaction processing.

Setup
1. Clone the repository:
2. Configure your database in 'appsettings.json'.
3. Run the application.

Usage

- Use API endpoints to manage bets.
- Worker service handles payment transactions asynchronously.
