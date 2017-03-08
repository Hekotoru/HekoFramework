namespace PHttp
{

    public enum ClientState
    {
        ReadingProlog,
        ReadingHeaders,
        ReadingContent,
        WritingHeaders,
        WritingContent,
        Closed
    }
}