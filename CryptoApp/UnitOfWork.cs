using CryptoApp.Repositories;

namespace CryptoApp
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IWalletRepository WalletRepository { get; }
        ICryptoRepository CryptoRepository { get; }
        ITradeRepository TradeRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        IProfitRepository ProfitRepository { get; }
        Task SaveAsync();
    }

    public class ProductionUnitOfWork : IUnitOfWork
    {
        private readonly CryptoDbContext _context;
        IUserRepository _userRepository;
        IWalletRepository _walletRepository;
        ICryptoRepository _cryptoRepository;
        ITradeRepository _tradeRepository;
        ITransactionRepository _transactionRepository;
        IProfitRepository _profitRepository;
        public ProductionUnitOfWork(CryptoDbContext context, IUserRepository userRepository, IWalletRepository walletRepository, 
            ICryptoRepository cryptoRepository, ITradeRepository tradeRepository, ITransactionRepository transactionRepository, IProfitRepository profitRepository)
        {
            _context = context;
            _userRepository = userRepository;
            _walletRepository = walletRepository;
            _cryptoRepository = cryptoRepository;
            _tradeRepository = tradeRepository;
            _transactionRepository = transactionRepository;
            _profitRepository = profitRepository;
        }

        public IUserRepository UserRepository => _userRepository;
        public IWalletRepository WalletRepository => _walletRepository;
        public ICryptoRepository CryptoRepository => _cryptoRepository;
        public ITradeRepository TradeRepository => _tradeRepository;
        public ITransactionRepository TransactionRepository => _transactionRepository;
        public IProfitRepository ProfitRepository => _profitRepository;
        public void Dispose()
        {
            _context.Dispose();
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
