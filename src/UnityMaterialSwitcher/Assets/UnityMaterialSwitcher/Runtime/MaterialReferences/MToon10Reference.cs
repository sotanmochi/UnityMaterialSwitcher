namespace MaterialSwitcher
{
    public class MToon10Reference : MaterialReference
    {
        public override string GetShaderName()
        {
            return RenderPipelineUtility.GetRenderPipelineType() switch
            {
                RenderPipelineType.UniversalRenderPipeline => "VRM10/Universal Render Pipeline/MToon10",
                _ => "VRM10/MToon10"
            };
        }
    }
}