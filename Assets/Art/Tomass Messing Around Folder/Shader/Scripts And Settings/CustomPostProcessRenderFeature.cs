using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

[System.Serializable]
public class CustomPostProcessRenderFeature : ScriptableRendererFeature
{
    [SerializeField]
    private Shader m_bloomShader;
    [SerializeField]
    private Shader m_compositeShader;

    private Material m_bloomMaterial;
    private Material m_compositeMaterial;



    CustomPostProcessPass m_customPass;

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_customPass);
    }

    public override void Create()
    {
        m_bloomMaterial = CoreUtils.CreateEngineMaterial(m_bloomShader);
        m_compositeMaterial = CoreUtils.CreateEngineMaterial(m_compositeShader);


        m_customPass = new CustomPostProcessPass(m_bloomMaterial, m_compositeMaterial);
    }
    
    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(m_bloomMaterial);
        CoreUtils.Destroy(m_compositeMaterial);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            m_pass.ConfigureInput(ScriptableRenderPassInput.Depth);
            m_pass.ConfigureInput(ScriptableRenderPassInput.Color);
            m_pass.SetTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
        }
    }
}
