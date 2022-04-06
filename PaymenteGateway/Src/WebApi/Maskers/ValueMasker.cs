namespace WebApi.Maskers
{
    public class ValueMasker : IValueMasker
    {
        public string Mask(string value) => "******";
    }
}
