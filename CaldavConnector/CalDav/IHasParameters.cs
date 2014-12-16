using System.Collections.Specialized;

namespace CaldavConnector.CalDav {
	public interface IHasParameters {
		NameValueCollection GetParameters();
		void Deserialize(string value, NameValueCollection parameters);
	}
}
