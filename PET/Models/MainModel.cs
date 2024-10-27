using PET.Services.HttpClient;

namespace PET.Models
{
    public class MainModel
    {      
        private enum Account : byte
        {
            AddOrUpdateAccount,
            GetAccount,
            GetCodeFromEmail,
            Login            
        }

        private enum BankAccount : byte
        {
            GetBankAccount,
            ToUpAccount
        }

        private enum GeneralInformation : byte
        {
            GetCurrencyRate
        }
        
        public UserDTO AccountVM { get; set; } = new();
        public BankAccountDTO BankAcc { get; set; }

        public Currencies currencies { get; private set; } = new(0.0f, 0.0f);

        private readonly IHttpClient httpClient ;

        public MainModel(IHttpClient httpClient) => this.httpClient = httpClient;  
                      
        public async Task GetCurrentUser()
        {
            var response = await httpClient.GetAsync<UserDTO>(nameof(Account), nameof(Account.GetAccount));

            if (response is not null)
                AccountVM = response;                            
        }

        public async Task<bool> Login(string login, string password)
        {
            var response = await httpClient.GetAsync<UserDTO>(nameof(Account), nameof(Account.Login), login, password);
            if (response is null)
                return false;

            AccountVM = response;
            await Console.Out.WriteLineAsync(AccountVM.JWT);
            return true;
        }

        public async Task<ResponseMessages> GetCodeFroEmail()
        {
            return await httpClient.GetAsync<ResponseMessages>(nameof(Account), nameof(Account.GetCodeFromEmail), AccountVM.EmailAdress, AccountVM.Telephone.ToString());            
        }

        public async Task<ResponseMessages> CreateOrUpdateAccount()
        {
            return await httpClient.PostGetJsonAsync<UserDTO, ResponseMessages>(AccountVM, nameof(Account), nameof(Account.AddOrUpdateAccount));                   
        }

        public async Task GetBankAccount()
        {
            BankAcc = await httpClient.GetAsync<BankAccountDTO>(nameof(BankAccount), nameof(BankAccount.GetBankAccount));           
        }

        public async Task<bool> TopUpAccount(int sum)
        {
            var res = await httpClient.GetAsync<bool>(nameof(BankAccount), nameof(BankAccount.ToUpAccount), sum.ToString());
            if(res)
            {
                BankAcc.Balance += sum;
                return true;
            }
            else
            {
                return false;
            }
        }       

        public async Task GetCurrencyRateAsync()
        {
            var res = await httpClient.GetAsync<Currencies>(nameof(GeneralInformation), nameof(GeneralInformation.GetCurrencyRate));
            if (res is not null)
                currencies = res;
        }
    }
}
