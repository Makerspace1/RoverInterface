namespace RoverBlazor.Data
{
    public struct RoverMessage
    {
        public string Message;
        public string Color;

        public RoverMessage(string message, string color = "#000")
        {
            Message = message;
            Color = color;
        }
    }
}
