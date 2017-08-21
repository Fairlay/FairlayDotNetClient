namespace FairlayDotNetClient.PrivateApi
{
	public enum PrivateApiErrorType
	{
		None,
		General,
		/// <summary>
		/// There was an error in a subtask of a bulk change order reqeust
		/// </summary>
		SubtaskBulkChangeOrderReqeust
	}
}