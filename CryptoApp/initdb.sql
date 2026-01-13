INSERT INTO Cryptos (Symbol, Price, Supply) VALUES 
('BTC', 67500.00, 1000),
('ETH', 3200.00, 5000),
('BNB', 580.00, 10000),
('SOL', 145.00, 15000),
('XRP', 0.52, 500000),
('ADA', 0.45, 300000),
('DOGE', 0.12, 1000000),
('DOT', 6.80, 50000),
('MATIC', 0.58, 200000),
('LINK', 14.20, 40000),
('AVAX', 35.50, 25000),
('ATOM', 8.90, 30000),
('UNI', 7.60, 35000),
('LTC', 84.00, 20000),
('XLM', 0.11, 400000);

INSERT INTO Users (Username, Email, Password) VALUES 
('johndoe', 'john.doe@email.com', 'password123'),
('janedoe', 'jane.doe@email.com', 'password123'),
('cryptoking', 'crypto.king@email.com', 'password123');

INSERT INTO Wallets (UserId, Balance) VALUES 
(1, 10000.00),
(2, 25000.00),
(3, 50000.00);

INSERT INTO Portfolios (WalletId, CryptoId, Amount) VALUES 
(1, 1, 0.5),
(1, 2, 2.0),
(2, 1, 0.25),
(2, 5, 1000.0),
(3, 1, 1.5),
(3, 2, 10.0),
(3, 4, 50.0);

INSERT INTO Transactions (UserId, CryptoId, Amount, CurrentPrice, Type, Date) VALUES 
(1, 1, 0.5, 65000.00, 0, '2025-01-03'),
(1, 2, 2.0, 3100.00, 0, '2025-01-06'),
(2, 1, 0.25, 66000.00, 0, '2025-01-08'),
(3, 1, 1.5, 64000.00, 0, '2024-12-30');