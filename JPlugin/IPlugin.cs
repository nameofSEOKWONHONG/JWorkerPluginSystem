namespace JPlugin
{
    public interface IPlugin
    {
        /// <summary>
        ///     set request parameter
        /// </summary>
        /// <param name="requst"></param>
        void SetRequest<TRequest>(TRequest request);

        /// <summary>
        ///     request validate
        /// </summary>
        /// <returns></returns>
        bool Validate();

        /// <summary>
        ///     pre execute
        /// </summary>
        /// <returns></returns>
        bool PreExecute();

        /// <summary>
        ///     main execute
        /// </summary>
        /// <returns></returns>
        object Execute();

        /// <summary>
        ///     after execute
        /// </summary>
        /// <returns></returns>
        bool AfterExecute();
    }
}