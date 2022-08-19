namespace Mosaico.Application.Wallet.Commands.Checkout.PrevalidatePurchase
{
    public class PrevalidatePurchaseCommandResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public bool IsPhoneNumberRequired { get; set; }
    }
}