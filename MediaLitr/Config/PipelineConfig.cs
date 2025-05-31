using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MediaLitr.Test")]
namespace MediaLitr.Config
{
    internal class PipelineConfig
    {
        public List<Type> GenericPipelines { get; set; }
    }
}
