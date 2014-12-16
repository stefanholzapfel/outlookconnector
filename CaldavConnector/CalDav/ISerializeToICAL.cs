using System.IO;

namespace CaldavConnector.CalDav {
	public interface ISerializeToICAL {
		void Deserialize(TextReader rdr, Serializer serializer);
		void Serialize(TextWriter wrtr);
	}
}
