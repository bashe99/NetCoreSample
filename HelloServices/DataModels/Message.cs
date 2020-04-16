
namespace HelloServices.DataModels
{
    public class Message: IMessage
    {
        string value { get; set; }

        public Message()
        {
            this.value = nameof(Message);
        }

        public Message(string value)
        {
            this.value = value;
        }

        public string getValue()
        {
            return $"value: {this.value}";
        }
    }
}
