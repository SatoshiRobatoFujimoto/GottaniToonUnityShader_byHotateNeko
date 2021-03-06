﻿using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
public class GottaniEditor : ShaderGUI {
    private static bool AutoShaderChange = true;
	private static string[] LayerLabels = new string[3]{"Label0","Label1","Label2"};
    public enum BlendMode {
        Opaque,                 // 不透明
        Transparent,            // 半透明
        Additive,               // 加算
        AdditiveTransparent,    // 加算半透明
    }
    public enum BloomBlendMode {
        Bloom,
        HardFur,
        SoftFur,
    }
    public enum StencilMode {
        Keep,
        Mask,
        Out,
        In,
    }

    static bool isEnglishMode = false;

	private MaterialProperty blendProp, stencilProp, bloomBlendProp;

    static bool shaderTagFactor = false; 
    static bool shaderTagBlendFactor = false; 
    static bool shaderTagStencillactor = false; 
    static bool addvanceStencillactor = false; 
    static bool shaderTagCullingFactor = false; 
    static bool addvanceCullingFactor = false; 
    static bool mainFactor = true;
    static bool maintexFactor = false;
    static bool hsvExchangeFactor = false;
    static bool hsvExchangeColor = false;
    static bool overlayTextureFactor = false;
    static bool toonFactor = false;
    static bool grabPassFactor = false;
    static bool pbrFactor = false;
    static bool virtualLightFactor = false;
    static bool fresnelFactor = false;
    static bool outlineFactor = false;
    static bool emissionFactor = false;
    static bool wireframeFactor = false;
    static bool parallaxFactor = false;
    static bool geometryFactor = false;
    static bool doppelgengerFactor = false;
    static bool gpupFactor = false;
    static bool matrixFactor = false;
    static bool fakebloomFactor = false;
    static bool noiseGeneratoFactor = false;
    static bool settingFactor = false;
    // プロパティを取得
    MaterialProperty pSrcBlend;
    MaterialProperty pDstBlend;
    
    MaterialProperty pZWrite;
    MaterialProperty pZTest;
    MaterialProperty pCullMode;
    MaterialProperty pAlphaCut;

    MaterialProperty pStencil;
    MaterialProperty pReadMask;
    MaterialProperty pWriteMask;
    MaterialProperty pStencilComp;
    MaterialProperty pStencilPass;
    MaterialProperty pStencilFail;
    MaterialProperty pStencilZFail;

    MaterialProperty pMirrorCulling;
    MaterialProperty pCameraCulling;
    MaterialProperty pBaseMirrorCulling;
    MaterialProperty pBaseCameraCulling;
    MaterialProperty pAddMirrorCulling;
    MaterialProperty pAddCameraCulling;
    MaterialProperty pShadowMirrorCulling;
    MaterialProperty pShadowCameraCulling;
    MaterialProperty pBloomMirrorCulling;
    MaterialProperty pBloomCameraCulling;
    MaterialProperty pCameraResolution;

    MaterialProperty pMainTex;
    MaterialProperty pUVScroll;
    MaterialProperty pColor;
    MaterialProperty pVertexColorMultiple;

    MaterialProperty pMirrorMode;
    MaterialProperty pReflectionTex0;
    MaterialProperty pReflectionTex1;
    
    MaterialProperty pHSVMask;
    MaterialProperty pHSVPosMask;
    MaterialProperty pExchangeColor;
    MaterialProperty pExchangeThreshold;
    MaterialProperty pH;
    MaterialProperty pS;
    MaterialProperty pV;
    MaterialProperty pColorStepFactor;
    MaterialProperty pWireframeWidth;
    MaterialProperty pWireframeColor;
    MaterialProperty pWireframeBlend;
    MaterialProperty pOverrideTexture;
    MaterialProperty pOverridePosMask;
    MaterialProperty pOverrideColor;
    MaterialProperty pOverrideH;
    MaterialProperty pOverrideS;
    MaterialProperty pOverrideV;

    MaterialProperty pNearClip;
    MaterialProperty pFarClip;
    MaterialProperty pClipThreshold;

    MaterialProperty pAlpha2Grab;
    MaterialProperty pGrabBlend;
    MaterialProperty pGrabMul;
    MaterialProperty pGrabAdd;
    MaterialProperty pRelativeRefractionIndex;
    MaterialProperty pRefractDistance;
    MaterialProperty pChromaticAberrationR;
    MaterialProperty pChromaticAberrationG;
    MaterialProperty pChromaticAberrationB;

    MaterialProperty pGrabMosicFactor;
    MaterialProperty pGrabColorStepFactor;
    MaterialProperty pGrabH;
    MaterialProperty pGrabS;
    MaterialProperty pGrabV;


    MaterialProperty pSubTex;
    MaterialProperty pSubColor;
    MaterialProperty pBackTex;
    MaterialProperty pAlphaMask;
    MaterialProperty pDisolveMask;
    MaterialProperty pShadowTex;
    MaterialProperty pBackTextureLevel;
    MaterialProperty pAlphaMaskLevel;
    MaterialProperty pDisolveSpeed;

    MaterialProperty pShadeColor0;
    MaterialProperty pShadeColor1;
    MaterialProperty pShadowTextureLevel;

    MaterialProperty pBumpMap;
    MaterialProperty pBumpScale;
    MaterialProperty pVirtualLight;
    MaterialProperty pVirtualGI;
    MaterialProperty pFlatShading;
    MaterialProperty pLambert;
    MaterialProperty pLightColorAttenuation;
    MaterialProperty pReceiveShadowRate;
    MaterialProperty pIndirectLightIntensity;
    MaterialProperty pMaxPointLightEfect;

    MaterialProperty pToon;
    MaterialProperty pToonMask;
    MaterialProperty pShadeColor;
    MaterialProperty pShadeShift0;
    MaterialProperty pShadeToony0;
    MaterialProperty pShadeShift1;
    MaterialProperty pShadeToony1;
    
    MaterialProperty pFresnelMask;

    MaterialProperty pRimColor;
    MaterialProperty pRimFresnelPower;
    MaterialProperty pRimLift;
    MaterialProperty pRimPower;
    MaterialProperty pRimFresnel;

    MaterialProperty pMatCap;
    MaterialProperty pMatCapColor;
    MaterialProperty pMatcapMode;

    MaterialProperty pOutlineCullMode;
    MaterialProperty pOutlineBase;
    MaterialProperty pOutlineMask;
    MaterialProperty pOutlineTex;
    MaterialProperty pOutlineColor;
    MaterialProperty pOutlineWidth;
    MaterialProperty pOutlineMainTexBlend;

    MaterialProperty pRoughnessMetallicMap;
    MaterialProperty pMetalic;
    MaterialProperty pRoughness;

    MaterialProperty pEmissionTex;
    MaterialProperty pEmissionMask;
    MaterialProperty pEmissionColor;
    MaterialProperty pEmissionVelocity;
    MaterialProperty pEmissionScroll;
    MaterialProperty pEmissionWidth;

    MaterialProperty pAlpha2Parallax;
    MaterialProperty pParallaxTexture;
    MaterialProperty pParallaxMask;
    MaterialProperty pParallaxColor;
    MaterialProperty pParallaxDepth;
    MaterialProperty pParallaxBlend;
    MaterialProperty pParallaxMul;
    MaterialProperty pParallaxAdd;
    MaterialProperty pParallaxScroll;

    MaterialProperty pPositionMemory;
    MaterialProperty pPositionPower;
    MaterialProperty pPositionFactor;
    MaterialProperty pObjPosFactor;
    MaterialProperty pObjPosNoise;
    MaterialProperty pMemoryScroll;
    MaterialProperty pGrabity;
    MaterialProperty pExtrusionFactor;
    MaterialProperty pExtrusionNoise;
    MaterialProperty pRotationFactor;
    MaterialProperty pRotationNoise;
    MaterialProperty pScaleFactor;
    MaterialProperty pScaleNoise;

    MaterialProperty pParticleFactor;
    MaterialProperty pPositionNoise;
    MaterialProperty pParticleColorFactor;
    MaterialProperty pParticleColor;
    MaterialProperty pParticleSize;

    MaterialProperty pTessFactor;
    MaterialProperty pTessMap;

    MaterialProperty pBloomZWrite;
    MaterialProperty pBloomSrcBlend;
    MaterialProperty pBloomDstBlend;
    MaterialProperty pBloomCullMode;
    MaterialProperty pBloomBase;
    MaterialProperty pBloomMask;
    MaterialProperty pFurNoise;
    MaterialProperty pFurSaturate;
    MaterialProperty pBloomTex;
    MaterialProperty pBloomCol;
    MaterialProperty pBloomRange;
    MaterialProperty pBloomBrightness;
    MaterialProperty pBloomKnee;
    MaterialProperty pBloomSoftknee;
    MaterialProperty pBloomThreshold;
    MaterialProperty pFurWindPower;
    MaterialProperty pFurWindSpeed;
    MaterialProperty pFurWindType;
    MaterialProperty pFurGravity;
    MaterialProperty pBloomLightEffect;
    MaterialProperty pBloomMainTexBlend;

    MaterialProperty pModel2World;
    MaterialProperty pModelRot;
    MaterialProperty pModelScale;
    MaterialProperty pModelPos;
    MaterialProperty pWorld2View;
    MaterialProperty pCameraRot;
    MaterialProperty pCameraScale;
    MaterialProperty pCameraPos;
    MaterialProperty pBillBoard;
    MaterialProperty pBillBoardRot;
    MaterialProperty pBillBoardScale;
    MaterialProperty pBillBoardPos;

    MaterialProperty pIS_DOPPELGENGER;
    MaterialProperty pDoppelganger;
    MaterialProperty pDoppelgangerPos;
    MaterialProperty pNOISE_GENERATE;
    MaterialProperty pFBM;
    MaterialProperty pNoiseMode;
    MaterialProperty pNoise_ST;
    MaterialProperty pNoiseScroll;
  

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        // プロパティを取得
        blendProp = FindProperty("_BlendMode", properties);
        stencilProp = FindProperty("_StencilMode", properties);
        bloomBlendProp = FindProperty("_BloomBlendMode",properties);

        pSrcBlend = FindProperty("_SrcBlend", properties);
        pDstBlend = FindProperty("_DstBlend", properties);
        
        pZWrite = FindProperty("_ZWrite", properties);
        pZTest = FindProperty("_ZTest", properties);
        pCullMode = FindProperty("_CullMode", properties);
        pAlphaCut = FindProperty("_AlphaCut", properties);

        pStencil = FindProperty("_Stencil", properties);
        pReadMask = FindProperty("_ReadMask", properties);
        pWriteMask = FindProperty("_WriteMask", properties);
        pStencilComp = FindProperty("_StencilComp", properties);
        pStencilPass = FindProperty("_StencilPass", properties);
        pStencilFail = FindProperty("_StencilFail", properties);
        pStencilZFail = FindProperty("_StencilZFail", properties);

        pMirrorCulling = FindProperty("_MirrorCulling", properties);
        pCameraCulling = FindProperty("_CameraCulling", properties);
        pBaseMirrorCulling = FindProperty("_BaseMirrorCulling", properties);
        pBaseCameraCulling = FindProperty("_BaseCameraCulling", properties);
        pAddMirrorCulling = FindProperty("_AddMirrorCulling", properties);
        pAddCameraCulling = FindProperty("_AddCameraCulling", properties);
        pShadowMirrorCulling = FindProperty("_ShadowMirrorCulling", properties);
        pShadowCameraCulling = FindProperty("_ShadowCameraCulling", properties);
        pBloomMirrorCulling = FindProperty("_BloomMirrorCulling", properties);
        pBloomCameraCulling = FindProperty("_BloomCameraCulling", properties);
        pCameraResolution = FindProperty("_CameraResolution", properties);

        pMainTex = FindProperty("_MainTex", properties);
        pUVScroll = FindProperty("_UVScroll", properties);
        pColor = FindProperty("_Color", properties);
        pVertexColorMultiple = FindProperty("_VertexColorMultiple", properties);

        pMirrorMode = FindProperty("_MirrorMode",properties);
        pReflectionTex0 = FindProperty("_ReflectionTex0", properties);
        pReflectionTex1 = FindProperty("_ReflectionTex1", properties);
        
        pHSVMask = FindProperty("_HSVMask", properties);
        pHSVPosMask = FindProperty("_HSVPosMask", properties);
        pExchangeColor = FindProperty("_ExchangeColor", properties);
        pExchangeThreshold = FindProperty("_ExchangeThreshold", properties);
        pH = FindProperty("_H", properties);
        pS = FindProperty("_S", properties);
        pV = FindProperty("_V", properties);
        pColorStepFactor = FindProperty("_ColorStepFactor", properties);
        pWireframeWidth = FindProperty("_WireframeWidth", properties);
        pWireframeColor = FindProperty("_WireframeColor", properties);
        pWireframeBlend = FindProperty("_WireframeBlend", properties);
        pOverrideTexture = FindProperty("_OverrideTexture", properties);
        pOverridePosMask = FindProperty("_OverridePosMask", properties);
        pOverrideColor = FindProperty("_OverrideColor", properties);
        pOverrideH = FindProperty("_OverrideH", properties);
        pOverrideS = FindProperty("_OverrideS", properties);
        pOverrideV = FindProperty("_OverrideV", properties);

        pNearClip = FindProperty("_NearClip", properties);
        pFarClip = FindProperty("_FarClip", properties);
        pClipThreshold = FindProperty("_ClipThreshold", properties);

        pAlpha2Grab = FindProperty("_Alpha2Grab", properties);
        pGrabBlend = FindProperty("_GrabBlend", properties);
        pGrabMul = FindProperty("_GrabMul", properties);
        pGrabAdd = FindProperty("_GrabAdd", properties);
        pRelativeRefractionIndex = FindProperty("_RelativeRefractionIndex", properties);
        pRefractDistance = FindProperty("_RefractDistance", properties);
        pChromaticAberrationR = FindProperty("_ChromaticAberrationR", properties);
        pChromaticAberrationG = FindProperty("_ChromaticAberrationG", properties);
        pChromaticAberrationB = FindProperty("_ChromaticAberrationB", properties);

        pGrabMosicFactor = FindProperty("_GrabMosicFactor", properties);
        pGrabColorStepFactor = FindProperty("_GrabColorStepFactor", properties);
        pGrabH = FindProperty("_GrabH", properties);
        pGrabS = FindProperty("_GrabS", properties);
        pGrabV = FindProperty("_GrabV", properties);

        pSubTex = FindProperty("_SubTex", properties);
        pSubColor = FindProperty("_SubColor", properties);
        pBackTextureLevel = FindProperty("_Tex2BackTextureLevel", properties);
        pBackTex = FindProperty("_BackTex",properties);
        pAlphaMask = FindProperty("_AlphaMask",properties);
        pDisolveMask = FindProperty("_DisolveMask",properties);
        pShadowTex = FindProperty("_ShadowTex",properties);

        pAlphaMaskLevel = FindProperty("_Tex2AlphaMask",properties);
        pDisolveSpeed = FindProperty("_DisolveSpeed",properties);

        pShadeColor = FindProperty("_ShadeColor",properties);

        pShadeColor0 = FindProperty("_ShadeColor0", properties);
        pShadeColor1 = FindProperty("_ShadeColor1", properties);
        pShadowTextureLevel = FindProperty("_Tex2ShadowTextureLevel", properties);

        pBumpMap = FindProperty("_BumpMap", properties);
        pBumpScale = FindProperty("_BumpScale", properties);
        pVirtualLight = FindProperty("_VirtualLight", properties);
        pVirtualGI = FindProperty("_VirtualGI", properties);
        pFlatShading = FindProperty("_FlatShading", properties);
        pLambert = FindProperty("_Lambert", properties);
        pLightColorAttenuation = FindProperty("_LightColorAttenuation", properties);
        pReceiveShadowRate = FindProperty("_ReceiveShadowRate", properties);
        pIndirectLightIntensity = FindProperty("_IndirectLightIntensity", properties);
        pMaxPointLightEfect = FindProperty("_MaxPointLightEfect", properties);

        pToonMask = FindProperty("_ToonMask",properties);
        pToon = FindProperty("_Toon", properties);
        pShadeShift0 = FindProperty("_ShadeShift0", properties);
        pShadeToony0 = FindProperty("_ShadeToony0", properties);
        pShadeShift1 = FindProperty("_ShadeShift1", properties);
        pShadeToony1 = FindProperty("_ShadeToony1", properties);
        
        pFresnelMask = FindProperty("_FresnelMask", properties);

        pRimColor = FindProperty("_RimColor", properties);
        pRimFresnelPower = FindProperty("_RimFresnelPower", properties);
        pRimLift = FindProperty("_RimLift", properties);
        pRimPower = FindProperty("_RimPower", properties);
        pRimFresnel = FindProperty("_RimFresnel", properties);

        pMatCap = FindProperty("_MatCap", properties);
        pMatCapColor = FindProperty("_MatCapColor", properties);
        pMatcapMode = FindProperty("_MatcapMode",properties);

        pOutlineCullMode = FindProperty("_OutlineCullMode", properties);
        pOutlineBase = FindProperty("_OutlineBase", properties);
        pOutlineMask = FindProperty("_OutlineMask", properties);
        pOutlineTex = FindProperty("_OutlineTex", properties);
        pOutlineColor = FindProperty("_OutlineColor", properties);
        pOutlineWidth = FindProperty("_OutlineWidth", properties);
        pOutlineMainTexBlend = FindProperty("_OutlineMainTexBlend", properties);

        pRoughnessMetallicMap = FindProperty("_RoughnessMetallicMap", properties);
        pMetalic = FindProperty("_Metalic", properties);
        pRoughness = FindProperty("_Roughness", properties);

        pEmissionTex = FindProperty("_EmissionTex", properties);
        pEmissionMask = FindProperty("_EmissionMask", properties);
        pEmissionColor = FindProperty("_EmissionColor", properties);
        pEmissionVelocity = FindProperty("_EmissionVelocity", properties);
        pEmissionScroll = FindProperty("_EmissionScroll", properties);
        pEmissionWidth = FindProperty("_EmissionWidth", properties);

        pAlpha2Parallax = FindProperty("_Alpha2Parallax", properties);
        pParallaxTexture = FindProperty("_ParallaxTexture", properties);
        pParallaxMask = FindProperty("_ParallaxMask", properties);
        pParallaxColor = FindProperty("_ParallaxColor", properties);
        pParallaxDepth = FindProperty("_ParallaxDepth", properties);
        pParallaxBlend = FindProperty("_ParallaxBlend", properties);
        pParallaxMul = FindProperty("_ParallaxMul", properties);
        pParallaxAdd = FindProperty("_ParallaxAdd", properties);
        pParallaxScroll = FindProperty("_ParallaxScroll", properties);

        pPositionMemory = FindProperty("_PositionMemory", properties);
        pPositionPower = FindProperty("_PositionPower", properties);
        pPositionFactor = FindProperty("_PositionFactor", properties);
        pObjPosFactor = FindProperty("_ObjPosFactor",properties);
        pObjPosNoise = FindProperty("_ObjPosNoise",properties);
        pMemoryScroll = FindProperty("_MemoryScroll", properties);
        pGrabity = FindProperty("_Grabity", properties);
        pExtrusionFactor = FindProperty("_ExtrusionFactor", properties);
        pExtrusionNoise = FindProperty("_ExtrusionNoise", properties);
        pRotationFactor = FindProperty("_RotationFactor", properties);
        pRotationNoise = FindProperty("_RotationNoise", properties);
        pScaleFactor = FindProperty("_ScaleFactor", properties);
        pScaleNoise = FindProperty("_ScaleNoise", properties);

        pParticleFactor = FindProperty("_ParticleFactor", properties);
        pPositionNoise = FindProperty("_PositionNoise", properties);
        pParticleColorFactor = FindProperty("_ParticleColorFactor", properties);
        pParticleColor = FindProperty("_ParticleColor", properties);
        pParticleSize = FindProperty("_ParticleSize", properties);

        pTessFactor = FindProperty("_TessFactor", properties);
        pTessMap = FindProperty("_TessMap", properties);

        pBloomZWrite = FindProperty("_BloomZWrite", properties);
        pBloomSrcBlend = FindProperty("_BloomSrcBlend", properties);
        pBloomDstBlend = FindProperty("_BloomDstBlend", properties);
        pBloomCullMode = FindProperty("_BloomCullMode", properties);
        pBloomBase = FindProperty("_BloomBase", properties);
        pBloomMask = FindProperty("_BloomMask", properties);
        pFurNoise = FindProperty("_FurNoise", properties);
        pFurSaturate = FindProperty("_FurSaturate", properties);
        pBloomTex = FindProperty("_BloomTex", properties);
        pBloomCol = FindProperty("_BloomCol", properties);
        pBloomRange = FindProperty("_BloomRange", properties);
        pBloomBrightness = FindProperty("_BloomBrightness", properties);
        pBloomKnee = FindProperty("_BloomKnee", properties);
        pBloomSoftknee = FindProperty("_BloomSoftknee", properties);
        pBloomThreshold = FindProperty("_BloomThreshold", properties);
        pFurWindPower = FindProperty("_FurWindPower", properties);
        pFurWindSpeed = FindProperty("_FurWindSpeed", properties);
        pFurWindType = FindProperty("_FurWindType", properties);
        pFurGravity = FindProperty("_FurGravity", properties);
        pBloomLightEffect = FindProperty("_BloomLightEffect",properties);
        pBloomMainTexBlend = FindProperty("_BloomMainTexBlend", properties);

        pModel2World = FindProperty("_Model2World", properties);
        pModelRot = FindProperty("_ModelRot", properties);
        pModelScale = FindProperty("_ModelScale", properties);
        pModelPos = FindProperty("_ModelPos", properties);
        pWorld2View = FindProperty("_World2View", properties);
        pCameraRot = FindProperty("_CameraRot", properties);
        pCameraScale = FindProperty("_CameraScale", properties);
        pCameraPos = FindProperty("_CameraPos", properties);
        pBillBoard = FindProperty("_BillBoard", properties);
        pBillBoardRot = FindProperty("_BillBoardRot", properties);
        pBillBoardScale = FindProperty("_BillBoardScale", properties);
        pBillBoardPos = FindProperty("_BillBoardPos", properties);

        pIS_DOPPELGENGER = FindProperty("_IS_DOPPELGENGER", properties);
        pDoppelganger = FindProperty("_Doppelganger", properties);
        pDoppelgangerPos = FindProperty("_DoppelgangerPos", properties);
        pNOISE_GENERATE = FindProperty("_NOISE_GENERATE", properties);
        pFBM = FindProperty("_FBM", properties);
        pNoiseMode = FindProperty("_NoiseMode", properties);
        pNoise_ST = FindProperty("_Noise_ST", properties);
        pNoiseScroll = FindProperty("_NoiseScroll", properties);
            
        var materials = materialEditor.targets.Cast<Material>();

        isEnglishMode = EditorGUILayout.Toggle("EnglishMode", isEnglishMode);
        if(isEnglishMode){
            shaderTagFactor = Foldout( "ShaderTag", shaderTagFactor);
            if(shaderTagFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                BlendMode b_mode = (BlendMode)this.blendProp.floatValue;
                StencilMode s_mode = (StencilMode)this.stencilProp.floatValue;
                EditorGUI.BeginChangeCheck();
                b_mode = (BlendMode)EditorGUILayout.Popup("Blend Mode", (int)b_mode, Enum.GetNames(typeof(BlendMode)));
                if (EditorGUI.EndChangeCheck()) {
                    this.blendProp.floatValue = (float)b_mode;
                    foreach (UnityEngine.Object obj in this.blendProp.targets)
                        this.SetupBlendMode(obj as Material, b_mode, s_mode);
                }

                materialEditor.RangeProperty(pAlphaCut,"Alpha Cut Out");

                EditorGUILayout.Space();
                
                EditorGUI.BeginChangeCheck();
                s_mode = (StencilMode)EditorGUILayout.Popup("Stencil Mode", (int)s_mode, Enum.GetNames(typeof(StencilMode)));
                if (EditorGUI.EndChangeCheck()) {
                    this.stencilProp.floatValue = (float)s_mode;
                    foreach (UnityEngine.Object obj in this.stencilProp.targets)
                        this.SetupStencilMode(obj as Material,b_mode, s_mode);
                }
                materialEditor.ShaderProperty(pStencil,"Stencil Ref");

                EditorGUILayout.Space();

                materialEditor.TexturePropertySingleLine(new GUIContent("Tesselation"),pTessMap,pTessFactor);
                materialEditor.RenderQueueField();

                shaderTagStencillactor = Foldout( "Stencil Advanced Setting", shaderTagStencillactor, EditorGUI.indentLevel);
                if(shaderTagStencillactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(pStencil,"Stencil Ref");
                    materialEditor.ShaderProperty(pReadMask,"ReadMask");
                    materialEditor.ShaderProperty(pWriteMask,"WriteMask");
                    materialEditor.ShaderProperty(pStencilComp,"Comp");
                    materialEditor.ShaderProperty(pStencilPass,"Pass");
                    materialEditor.ShaderProperty(pStencilFail,"Fail");
                    materialEditor.ShaderProperty(pStencilZFail,"ZTestFail");
                    EditorGUI.indentLevel--;
                }

                shaderTagBlendFactor = Foldout( "Blend Advanced Setting", shaderTagBlendFactor, EditorGUI.indentLevel);
                if(shaderTagBlendFactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(pSrcBlend,"BassPass SrcBlend");
                    materialEditor.ShaderProperty(pDstBlend,"BassPass DstBlend");
                    materialEditor.ShaderProperty(pBloomSrcBlend,"BloomPass SrcBlend");
                    materialEditor.ShaderProperty(pBloomDstBlend,"BloomPass DstBlend");
                    materialEditor.ShaderProperty(pZWrite,"BasePass ZWrite");
                    materialEditor.ShaderProperty(pZTest,"BassPass ZTest");
                    materialEditor.ShaderProperty(pBloomZWrite,"BloomPass ZWrite");
                    EditorGUI.indentLevel--;
                }
                shaderTagCullingFactor = Foldout("Culling Setting",shaderTagCullingFactor, EditorGUI.indentLevel);
                if(shaderTagCullingFactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(pCullMode,"BassPass Culling");
                    materialEditor.ShaderProperty(pOutlineCullMode,"OutlinePass Culling");
                    materialEditor.ShaderProperty(pBloomCullMode,"BloomPass Culling");
                    EditorGUILayout.Space();
                    materialEditor.ShaderProperty(pMirrorCulling,"Mirror Culling");
                    materialEditor.ShaderProperty(pCameraCulling,"VRChat Camera Culling");
                    addvanceCullingFactor = Foldout("Culling Advanced Setting",addvanceCullingFactor, EditorGUI.indentLevel);
                    if(addvanceCullingFactor)
                    {
                        EditorGUI.indentLevel++;
                        materialEditor.ShaderProperty(pBaseMirrorCulling,"BassPass MirrorCulling");
                        materialEditor.ShaderProperty(pBaseCameraCulling,"BasePass CameraCulling");
                        materialEditor.ShaderProperty(pAddMirrorCulling,"AddPass MirrorCulling");
                        materialEditor.ShaderProperty(pAddCameraCulling,"AddPass CameraCulling");
                        materialEditor.ShaderProperty(pShadowMirrorCulling,"ShadowPass MirrorCulling");
                        materialEditor.ShaderProperty(pShadowCameraCulling,"ShadowPass CameraCulling");
                        materialEditor.ShaderProperty(pBloomMirrorCulling,"BloomPass MirrorCulling");
                        materialEditor.ShaderProperty(pBloomCameraCulling,"BloomPass CameraCulling");
                        materialEditor.VectorProperty(pCameraResolution,"Camera Resolution");
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                    EditorGUILayout.LabelField("Distance Fade", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    materialEditor.FloatProperty(pNearClip,"NearClip");
                    materialEditor.FloatProperty(pFarClip,"FarClip");
                    materialEditor.RangeProperty(pClipThreshold,"Threshold");
                    EditorGUI.indentLevel--;
                }
            }}

            mainFactor = Foldout( "Texture", mainFactor);
            if(mainFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.TexturePropertySingleLine(new GUIContent("MainTexture"), pMainTex, pColor);
                materialEditor.TextureScaleOffsetProperty(pMainTex);
            }}

            maintexFactor = Foldout( "MainTexture Factor", maintexFactor);
            if(maintexFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.VectorProperty(pUVScroll,"UV Scroll");
                materialEditor.TexturePropertySingleLine(new GUIContent("Right Eye Texture"),pReflectionTex0,pMirrorMode);
                materialEditor.TexturePropertySingleLine(new GUIContent("Left Eye Texture"),pReflectionTex1);
                materialEditor.TexturePropertySingleLine(new GUIContent("Sub Texture"), pSubTex, pSubColor);
                materialEditor.TexturePropertySingleLine(new GUIContent("Shadow Texture"),pShadowTex,pShadowTextureLevel);
                materialEditor.TexturePropertySingleLine(new GUIContent("Back Texture"),pBackTex,pBackTextureLevel);
                materialEditor.TexturePropertySingleLine(new GUIContent("Toon Map"),pToonMask,pToon);
                materialEditor.TexturePropertySingleLine(new GUIContent("Alpha Mask"),pAlphaMask,pAlphaMaskLevel);
                materialEditor.TexturePropertySingleLine(new GUIContent("Disolve Mask"),pDisolveMask,pDisolveSpeed);
                EditorGUILayout.Space();
                materialEditor.RangeProperty(pVertexColorMultiple,"Vertex Color");
                hsvExchangeFactor = Foldout( "HSV Exchange", hsvExchangeFactor, EditorGUI.indentLevel);
                if(hsvExchangeFactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(pHSVPosMask,"ModelPosMask");
                    materialEditor.TexturePropertySingleLine(new GUIContent("Mask"),pHSVMask);
                    materialEditor.RangeProperty(pColorStepFactor,"Color Step Filter");
                    materialEditor.RangeProperty(pH,"H");
                    materialEditor.RangeProperty(pS,"S");
                    materialEditor.RangeProperty(pV,"V");
                    hsvExchangeColor = Foldout( "Color 2 Exchange", hsvExchangeColor, EditorGUI.indentLevel);
                    if(hsvExchangeColor)
                    {
                        EditorGUI.indentLevel++;
                        materialEditor.ColorProperty(pExchangeColor,"Sample Color");
                        materialEditor.RangeProperty(pExchangeThreshold,"Allowable Error");
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                }
                overlayTextureFactor = Foldout( "Texture Mask", overlayTextureFactor, EditorGUI.indentLevel);
                if(overlayTextureFactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(pOverridePosMask,"ModelPosMask");
                    materialEditor.TexturePropertySingleLine(new GUIContent("Override Texture"),pOverrideTexture,pOverrideColor);
                    materialEditor.TextureScaleOffsetProperty(pOverrideTexture);
                    materialEditor.RangeProperty(pOverrideH,"H");
                    materialEditor.RangeProperty(pOverrideS,"S");
                    materialEditor.RangeProperty(pOverrideV,"V");
                    EditorGUI.indentLevel--;
                }
            }}

            grabPassFactor = Foldout("GrabPass", grabPassFactor);
            if(grabPassFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(pAlpha2Grab,"Alpha 2 Grab");
                materialEditor.RangeProperty(pGrabBlend,"Blend");
                materialEditor.RangeProperty(pGrabAdd,"Add");
                materialEditor.RangeProperty(pGrabMul,"Multiply");
                materialEditor.RangeProperty(pRelativeRefractionIndex,"Refractive Index");
                materialEditor.RangeProperty(pRefractDistance,"Distance");
                materialEditor.RangeProperty(pGrabMosicFactor,"Mosaic Filter");
                materialEditor.RangeProperty(pGrabColorStepFactor,"Color Step Filter");
                materialEditor.RangeProperty(pGrabH,"H");
                materialEditor.RangeProperty(pGrabS,"S");
                materialEditor.RangeProperty(pGrabV,"V");
                materialEditor.VectorProperty(pChromaticAberrationR,"Red Chromatic Aberration");
                materialEditor.VectorProperty(pChromaticAberrationG,"Green Chromatic Aberration");
                materialEditor.VectorProperty(pChromaticAberrationB,"Blue Chromatic Aberration");
                EditorGUI.indentLevel--;
            }}

            pbrFactor = Foldout( "Lighting", pbrFactor);
            if(pbrFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUI.indentLevel++;
                materialEditor.TexturePropertySingleLine(new GUIContent("Toon Map"),pToonMask,pToon);
                materialEditor.TexturePropertySingleLine(new GUIContent("Bump Map"),pBumpMap,pBumpScale);
                materialEditor.TexturePropertySingleLine(new GUIContent("Metalic/Roughness/Shadow Mask"),pRoughnessMetallicMap);
                materialEditor.RangeProperty(pMetalic,"Metalic");
                materialEditor.RangeProperty(pRoughness,"Roughness");
                virtualLightFactor = Foldout( "Addvanced", virtualLightFactor, EditorGUI.indentLevel);
                if(virtualLightFactor)
                {
                    materialEditor.RangeProperty(pVirtualLight,"Virtual Directional Light");
                    materialEditor.RangeProperty(pVirtualGI,"Virtual Emviroment Light");
                    materialEditor.RangeProperty(pLightColorAttenuation,"Light Color Normalize");
                    materialEditor.RangeProperty(pReceiveShadowRate,"Shadow Power");
                    materialEditor.RangeProperty(pIndirectLightIntensity,"Emviroment Light Normalize");
                    materialEditor.RangeProperty(pMaxPointLightEfect,"Point Light Power");
                    materialEditor.RangeProperty(pLambert,"Half Lambert");
                    materialEditor.RangeProperty(pFlatShading,"Flat Shading");
                    materialEditor.RangeProperty(pBloomLightEffect,"Bloom Pass Lighting");
                }
                EditorGUI.indentLevel--;
            }}
            
            toonFactor = Foldout( "PBR 2 Toon",toonFactor);
            if(toonFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.TexturePropertySingleLine(new GUIContent("Toon Map"),pToonMask,pToon);
                materialEditor.TexturePropertySingleLine(new GUIContent("Custom Shadow"),pShadeColor);
                materialEditor.ColorProperty(pShadeColor0,"Shadow 1 Color");
                materialEditor.RangeProperty(pShadeShift0,"Shadow 1 Shift");
                materialEditor.RangeProperty(pShadeToony0,"Shadow 1 Toony");
                materialEditor.ColorProperty(pShadeColor1,"Shadow 2 Color");
                materialEditor.RangeProperty(pShadeShift1,"Shadow 2 Shift");
                materialEditor.RangeProperty(pShadeToony1,"Shadow 2 Toony");
                materialEditor.TexturePropertySingleLine(new GUIContent("Shadow Texture"),pShadowTex,pShadowTextureLevel);
            }}

            fresnelFactor = Foldout("Matcap & RimLight",fresnelFactor);
            if(fresnelFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.TexturePropertySingleLine(new GUIContent("Mask"),pFresnelMask);
                EditorGUILayout.LabelField("RimLight", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                    materialEditor.ColorProperty(pRimColor,"Rim Color");
                    materialEditor.RangeProperty(pRimLift,"Rim Shift");
                    materialEditor.RangeProperty(pRimPower,"Rim Power");
                    materialEditor.RangeProperty(pRimFresnel,"Fresnel");
                    materialEditor.RangeProperty(pRimFresnelPower,"Fresnel Power");
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("Matcap", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                    materialEditor.TexturePropertySingleLine(new GUIContent("Matcap Texture"),pMatCap,pMatCapColor,pMatcapMode);
                EditorGUI.indentLevel--;
            }}

            outlineFactor = Foldout( "Outline", outlineFactor);
            if(outlineFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.ShaderProperty(pOutlineBase,"Outline Mode");
                materialEditor.TexturePropertySingleLine(new GUIContent("Mask"), pOutlineMask);
                materialEditor.FloatProperty(pOutlineWidth,"Outline Width");
                materialEditor.TexturePropertySingleLine(new GUIContent("Outline Texture"),pOutlineTex,pOutlineColor);
                materialEditor.RangeProperty(pOutlineMainTexBlend,"MainTexture Blend");
            }}

            emissionFactor = Foldout( "Emission", emissionFactor);
            if(emissionFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.TexturePropertySingleLine(new GUIContent("Mask"), pEmissionMask);
                materialEditor.TexturePropertyWithHDRColor(new GUIContent("Emission Texture"), pEmissionTex, pEmissionColor, new ColorPickerHDRConfig(0, 100, 0.01f, 3.0f), false);
                materialEditor.TextureScaleOffsetProperty(pEmissionTex);
                materialEditor.VectorProperty(pEmissionVelocity,"Emissive Scroll(w→speed)");
                materialEditor.VectorProperty(pEmissionScroll,"Texture Scroll");
                materialEditor.RangeProperty(pEmissionWidth,"Emissive Width");
            }}
            
            wireframeFactor = Foldout( "Wire Frame", wireframeFactor);
            if(wireframeFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.ColorProperty(pWireframeColor,"Color");
                materialEditor.RangeProperty(pWireframeBlend,"MainTexture Blend");
                materialEditor.RangeProperty(pWireframeWidth,"Width");
            }}

            parallaxFactor = Foldout( "Parallax Mapping", parallaxFactor);
            if(parallaxFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.ShaderProperty(pAlpha2Parallax,"Alpha 2 Parallax");
                materialEditor.TexturePropertySingleLine(new GUIContent("Mask"),pParallaxMask);
                materialEditor.TexturePropertySingleLine(new GUIContent("Parallax Texture"),pParallaxTexture,pParallaxColor);
                materialEditor.VectorProperty(pParallaxScroll,"Texture Scroll");
                materialEditor.FloatProperty(pParallaxDepth,"Depth");
                materialEditor.RangeProperty(pParallaxBlend,"Blend");
                materialEditor.RangeProperty(pParallaxAdd,"Add");
                materialEditor.RangeProperty(pParallaxMul,"Multiply");
            }}

            geometryFactor = Foldout( "Vertex Shader", geometryFactor);
            if(geometryFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.TexturePropertySingleLine(new GUIContent("Vertex Position Memory"), pPositionMemory,pPositionPower);
                materialEditor.TextureScaleOffsetProperty(pPositionMemory);
                materialEditor.VectorProperty(pMemoryScroll,"Memory Scroll");
                materialEditor.RangeProperty(pPositionFactor,"Polygon or Vertex Bass");
                materialEditor.RangeProperty(pExtrusionFactor,"Extrusion Factor");
                materialEditor.RangeProperty(pExtrusionNoise,"Extrusion Noise");
                materialEditor.VectorProperty(pGrabity,"Grabity");
                materialEditor.RangeProperty(pRotationFactor,"Rotation Factor");
                materialEditor.RangeProperty(pRotationNoise,"Rotation Noise");
                materialEditor.RangeProperty(pScaleFactor,"Scale Factor");
                materialEditor.RangeProperty(pScaleNoise,"Scale Noise");

                doppelgengerFactor = Foldout( "Doppelgenger", doppelgengerFactor, EditorGUI.indentLevel);
                if(doppelgengerFactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(pIS_DOPPELGENGER,"Doppelgenger");
                    if(pIS_DOPPELGENGER.floatValue==1){
                        materialEditor.ShaderProperty(pDoppelganger,"Doppelgenger Num");
                        materialEditor.VectorProperty(pDoppelgangerPos,"Doppelgenger Vec");
                    }
                    EditorGUI.indentLevel--;
                }

                gpupFactor = Foldout( "GPU Particle", gpupFactor, EditorGUI.indentLevel);
                if(gpupFactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.RangeProperty(pParticleFactor,"Particle Factor");
                    materialEditor.ColorProperty(pParticleColor,"Color");
                    materialEditor.RangeProperty(pParticleColorFactor,"Texture Blend");
                    materialEditor.RangeProperty(pParticleSize,"Size");
                    if(pNOISE_GENERATE.floatValue==1) materialEditor.RangeProperty(pPositionNoise,"Noise");
                    materialEditor.RangeProperty(pObjPosFactor,"Object Position Factor");
                    materialEditor.RangeProperty(pObjPosNoise,"Object Position Noise");
                    EditorGUI.indentLevel--;
                }
                matrixFactor = Foldout( "Transformation MATRIX", matrixFactor, EditorGUI.indentLevel);
                if(matrixFactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(pModel2World,"Model2World");
                        EditorGUI.indentLevel++;
                        materialEditor.VectorProperty(pModelRot,"Rot");
                        materialEditor.VectorProperty(pModelScale,"Scale");
                        materialEditor.VectorProperty(pModelPos,"Pos");
                        EditorGUI.indentLevel--;
                    materialEditor.ShaderProperty(pWorld2View,"World2View");
                        EditorGUI.indentLevel++;
                        materialEditor.VectorProperty(pCameraRot,"Rot");
                        materialEditor.VectorProperty(pCameraScale,"Scale");
                        materialEditor.VectorProperty(pCameraPos,"Pos");
                        EditorGUI.indentLevel--;
                    materialEditor.ShaderProperty(pBillBoard,"Billbord");
                    if(pBillBoard.floatValue==1){
                        EditorGUI.indentLevel++;
                        materialEditor.VectorProperty(pBillBoardRot,"Rot");
                        materialEditor.VectorProperty(pBillBoardScale,"Scale");
                        materialEditor.VectorProperty(pBillBoardPos,"Pos");
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                }
            }}

            fakebloomFactor = Foldout( "Fake Bloom", fakebloomFactor);
            if(fakebloomFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.ShaderProperty(pBloomZWrite,"ZWrite");
                //materialEditor.ShaderProperty(pBloomDstBlend,"DstBlend");
                materialEditor.ShaderProperty(pBloomBase,"Bloom Base");

                //materialEditor.ShaderProperty(pBloomDstBlend,"ブレンドモード");
                materialEditor.TexturePropertySingleLine(new GUIContent("Mask"),pBloomMask);
                materialEditor.ShaderProperty(pFurSaturate,"MainTexture Normalize");
                //materialEditor.ShaderProperty(pFurNoise,"Noise");

                materialEditor.ShaderProperty(pBloomCullMode,"Culling Mode");
                BloomBlendMode bb_mode = (BloomBlendMode)this.bloomBlendProp.floatValue;
                EditorGUI.BeginChangeCheck();
                bb_mode = (BloomBlendMode)EditorGUILayout.Popup("Mode", (int)bb_mode, Enum.GetNames(typeof(BloomBlendMode)));
                if (EditorGUI.EndChangeCheck()) {
                    this.bloomBlendProp.floatValue = (float)bb_mode;
                    foreach (UnityEngine.Object obj in this.bloomBlendProp.targets)
                        this.SetupBloomBlendMode(obj as Material, bb_mode);
                }
                
                materialEditor.TexturePropertyWithHDRColor(new GUIContent("Bloom Texture"), pBloomTex, pBloomCol, new ColorPickerHDRConfig(0, 100, 0.01f, 3.0f), false);
                materialEditor.TextureScaleOffsetProperty(pBloomTex);
                materialEditor.RangeProperty(pBloomMainTexBlend,"MainTexture Blend");
                materialEditor.RangeProperty(pBloomRange,"Range");
                materialEditor.RangeProperty(pBloomBrightness,"Brightness");
                materialEditor.RangeProperty(pBloomKnee,"Knee");
                materialEditor.RangeProperty(pBloomSoftknee,"SoftKnee");
                materialEditor.RangeProperty(pBloomThreshold,"Threshold");
                materialEditor.RangeProperty(pFurWindPower,"WindPower");
                materialEditor.RangeProperty(pFurWindSpeed,"WindSpeed");
                materialEditor.RangeProperty(pFurWindType,"WindType");
                materialEditor.VectorProperty(pFurGravity,"Gravity");
                materialEditor.RangeProperty(pBloomLightEffect,"Bloom Pass Lighting");
            }}

            noiseGeneratoFactor = Foldout( "Noise (Don't Touch)", noiseGeneratoFactor);
            if(noiseGeneratoFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.ShaderProperty(pNOISE_GENERATE,"Noise Generate (DON'T PANIC)");
                materialEditor.ShaderProperty(pFBM,"FBM");
                materialEditor.ShaderProperty(pNoiseMode,"Noise Mode");
                materialEditor.VectorProperty(pNoise_ST,"Noise Scale");
                materialEditor.VectorProperty(pNoiseScroll,"Noise Scroll");
            }}
        }else{
            shaderTagFactor = Foldout( "シェーダータグ", shaderTagFactor);
            if(shaderTagFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                BlendMode b_mode = (BlendMode)this.blendProp.floatValue;
                StencilMode s_mode = (StencilMode)this.stencilProp.floatValue;
                EditorGUI.BeginChangeCheck();
                b_mode = (BlendMode)EditorGUILayout.Popup("ブレンドモード", (int)b_mode, Enum.GetNames(typeof(BlendMode)));
                if (EditorGUI.EndChangeCheck()) {
                    this.blendProp.floatValue = (float)b_mode;
                    foreach (UnityEngine.Object obj in this.blendProp.targets)
                        this.SetupBlendMode(obj as Material, b_mode, s_mode);
                }

                materialEditor.RangeProperty(pAlphaCut,"アルファカットアウト");

                EditorGUILayout.Space();
                
                EditorGUI.BeginChangeCheck();
                s_mode = (StencilMode)EditorGUILayout.Popup("ステンシルモード", (int)s_mode, Enum.GetNames(typeof(StencilMode)));
                if (EditorGUI.EndChangeCheck()) {
                    this.stencilProp.floatValue = (float)s_mode;
                    foreach (UnityEngine.Object obj in this.stencilProp.targets)
                        this.SetupStencilMode(obj as Material,b_mode, s_mode);
                }
                materialEditor.ShaderProperty(pStencil,"ステンシル値");

                EditorGUILayout.Space();

                materialEditor.TexturePropertySingleLine(new GUIContent("テッセレーション"),pTessMap,pTessFactor);
                materialEditor.RenderQueueField();

                shaderTagStencillactor = Foldout( "ステンシル設定", shaderTagStencillactor, EditorGUI.indentLevel);
                if(shaderTagStencillactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(pStencil,"ステンシル値");
                    materialEditor.ShaderProperty(pReadMask,"ReadMask");
                    materialEditor.ShaderProperty(pWriteMask,"WriteMask");
                    materialEditor.ShaderProperty(pStencilComp,"ステンシル値テスト");
                    materialEditor.ShaderProperty(pStencilPass,"合格");
                    materialEditor.ShaderProperty(pStencilFail,"不合格");
                    materialEditor.ShaderProperty(pStencilZFail,"ZTest不合格");
                    EditorGUI.indentLevel--;
                }

                shaderTagBlendFactor = Foldout( "ブレンド設定", shaderTagBlendFactor, EditorGUI.indentLevel);
                if(shaderTagBlendFactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(pSrcBlend,"ベースパス SrcBlend");
                    materialEditor.ShaderProperty(pDstBlend,"ベースパス DstBlend");
                    materialEditor.ShaderProperty(pBloomSrcBlend,"ブルーム SrcBlend");
                    materialEditor.ShaderProperty(pBloomDstBlend,"ブルーム DstBlend");
                    materialEditor.ShaderProperty(pZWrite,"ZWrite");
                    materialEditor.ShaderProperty(pZTest,"ZTest");
                    materialEditor.ShaderProperty(pBloomZWrite,"ブルーム ZWrite");
                    EditorGUI.indentLevel--;
                }
                shaderTagCullingFactor = Foldout("カリング設定",shaderTagCullingFactor, EditorGUI.indentLevel);
                if(shaderTagCullingFactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(pCullMode,"ベースパス カリング");
                    materialEditor.ShaderProperty(pOutlineCullMode,"アウトライン カリング");
                    materialEditor.ShaderProperty(pBloomCullMode,"ブルーム カリング");
                    EditorGUILayout.Space();
                    materialEditor.ShaderProperty(pMirrorCulling,"ミラーカリング");
                    materialEditor.ShaderProperty(pCameraCulling,"カメラカリング");
                    addvanceCullingFactor = Foldout("詳細",addvanceCullingFactor, EditorGUI.indentLevel);
                    if(addvanceCullingFactor)
                    {
                        EditorGUI.indentLevel++;
                        materialEditor.ShaderProperty(pBaseMirrorCulling,"ベースパス ミラーカリング");
                        materialEditor.ShaderProperty(pBaseCameraCulling,"ベースパス カメラカリング");
                        materialEditor.ShaderProperty(pAddMirrorCulling,"ポイントライトパス ミラーカリング");
                        materialEditor.ShaderProperty(pAddCameraCulling,"ポイントライトパス カメラカリング");
                        materialEditor.ShaderProperty(pShadowMirrorCulling,"シャドウパス ミラーカリング");
                        materialEditor.ShaderProperty(pShadowCameraCulling,"シャドウパス カメラカリング");
                        materialEditor.ShaderProperty(pBloomMirrorCulling,"ブルームパス ミラーカリング");
                        materialEditor.ShaderProperty(pBloomCameraCulling,"ブルームパス カメラカリング");
                        materialEditor.VectorProperty(pCameraResolution,"カメラ解像度");
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                    EditorGUILayout.LabelField("DistanceFade", EditorStyles.boldLabel);
                    EditorGUI.indentLevel++;
                    materialEditor.FloatProperty(pNearClip,"NearClip");
                    materialEditor.FloatProperty(pFarClip,"FarClip");
                    materialEditor.RangeProperty(pClipThreshold,"ノイズ");
                    EditorGUI.indentLevel--;
                }
            }}

            mainFactor = Foldout( "メインテクスチャ", mainFactor);
            if(mainFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.TexturePropertySingleLine(new GUIContent("メインテクスチャ"), pMainTex, pColor);
                materialEditor.TextureScaleOffsetProperty(pMainTex);
            }}

            maintexFactor = Foldout( "メインテクスチャ ファクター", maintexFactor);
            if(maintexFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.VectorProperty(pUVScroll,"UVスクロール");
                materialEditor.TexturePropertySingleLine(new GUIContent("右目テクスチャ"),pReflectionTex0,pMirrorMode);
                materialEditor.TexturePropertySingleLine(new GUIContent("左目テクスチャ"),pReflectionTex1);
                materialEditor.TexturePropertySingleLine(new GUIContent("サブテクスチャ"), pSubTex, pSubColor);
                materialEditor.TexturePropertySingleLine(new GUIContent("影にテクスチャをブレンド"),pShadowTex,pShadowTextureLevel);
                materialEditor.TexturePropertySingleLine(new GUIContent("裏面にテクスチャをブレンド"),pBackTex,pBackTextureLevel);
                materialEditor.TexturePropertySingleLine(new GUIContent("トゥーンマップ"),pToonMask,pToon);
                materialEditor.TexturePropertySingleLine(new GUIContent("アルファマスクレベル"),pAlphaMask,pAlphaMaskLevel);
                materialEditor.TexturePropertySingleLine(new GUIContent("ディゾルブスピードレベル"),pDisolveMask,pDisolveSpeed);
                EditorGUILayout.Space();
                materialEditor.RangeProperty(pVertexColorMultiple,"頂点カラー");
                hsvExchangeFactor = Foldout( "色変換", hsvExchangeFactor, EditorGUI.indentLevel);
                if(hsvExchangeFactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(pHSVPosMask,"モデル座標マスク");
                    materialEditor.TexturePropertySingleLine(new GUIContent("マスク"),pHSVMask);
                    materialEditor.RangeProperty(pColorStepFactor,"イラスト風");
                    materialEditor.RangeProperty(pH,"H");
                    materialEditor.RangeProperty(pS,"S");
                    materialEditor.RangeProperty(pV,"V");
                    hsvExchangeColor = Foldout( "色サンプルから変換", hsvExchangeColor, EditorGUI.indentLevel);
                    if(hsvExchangeColor)
                    {
                        EditorGUI.indentLevel++;
                        materialEditor.ColorProperty(pExchangeColor,"サンプル");
                        materialEditor.RangeProperty(pExchangeThreshold,"許容誤差");
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                }
                overlayTextureFactor = Foldout( "テクスチャ上書き", overlayTextureFactor, EditorGUI.indentLevel);
                if(overlayTextureFactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(pOverridePosMask,"モデル座標マスク");
                    materialEditor.TexturePropertySingleLine(new GUIContent("テクスチャ"),pOverrideTexture,pOverrideColor);
                    materialEditor.TextureScaleOffsetProperty(pOverrideTexture);
                    materialEditor.RangeProperty(pOverrideH,"H");
                    materialEditor.RangeProperty(pOverrideS,"S");
                    materialEditor.RangeProperty(pOverrideV,"V");
                    EditorGUI.indentLevel--;
                }
            }}

            grabPassFactor = Foldout("グラブパス", grabPassFactor);
            if(grabPassFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUI.indentLevel++;
                materialEditor.ShaderProperty(pAlpha2Grab,"透明度をグラブパスにする");
                materialEditor.RangeProperty(pGrabBlend,"ブレンド");
                materialEditor.RangeProperty(pGrabAdd,"足し算");
                materialEditor.RangeProperty(pGrabMul,"掛け算");
                materialEditor.RangeProperty(pRelativeRefractionIndex,"屈折率");
                materialEditor.RangeProperty(pRefractDistance,"焦点距離");
                materialEditor.RangeProperty(pGrabMosicFactor,"モザイク");
                materialEditor.RangeProperty(pGrabColorStepFactor,"イラスト風");
                materialEditor.RangeProperty(pGrabH,"H");
                materialEditor.RangeProperty(pGrabS,"S");
                materialEditor.RangeProperty(pGrabV,"V");
                materialEditor.VectorProperty(pChromaticAberrationR,"赤色収差");
                materialEditor.VectorProperty(pChromaticAberrationG,"緑色収差");
                materialEditor.VectorProperty(pChromaticAberrationB,"青色収差");
                EditorGUI.indentLevel--;
            }}

            pbrFactor = Foldout( "ライティング", pbrFactor);
            if(pbrFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                EditorGUI.indentLevel++;
                materialEditor.TexturePropertySingleLine(new GUIContent("トゥーンマップ"),pToonMask,pToon);
                materialEditor.TexturePropertySingleLine(new GUIContent("ノーマルマップ"),pBumpMap,pBumpScale);
                materialEditor.TexturePropertySingleLine(new GUIContent("メタリック/ラフネス/シャドウマスク"),pRoughnessMetallicMap);
                materialEditor.RangeProperty(pMetalic,"メタリック");
                materialEditor.RangeProperty(pRoughness,"ラフネス");
                virtualLightFactor = Foldout( "詳細設定", virtualLightFactor, EditorGUI.indentLevel);
                if(virtualLightFactor)
                {
                    materialEditor.RangeProperty(pVirtualLight,"仮想ディレクショナルライト");
                    materialEditor.RangeProperty(pVirtualGI,"仮想環境光");
                    materialEditor.RangeProperty(pLightColorAttenuation,"ライト色の平均化");
                    materialEditor.RangeProperty(pReceiveShadowRate,"影の濃さ");
                    materialEditor.RangeProperty(pIndirectLightIntensity,"ライト方向の平均化");
                    materialEditor.RangeProperty(pMaxPointLightEfect,"ポイントライトの影響");
                    materialEditor.RangeProperty(pLambert,"ハーフ ランバート");
                    materialEditor.RangeProperty(pFlatShading,"フラットシェーディング");
                    materialEditor.RangeProperty(pBloomLightEffect,"ブルームパスへのライトの影響");
                }
                EditorGUI.indentLevel--;
            }}
            
            toonFactor = Foldout( "トゥーンシェーディング",toonFactor);
            if(toonFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.TexturePropertySingleLine(new GUIContent("トゥーンマップ"),pToonMask,pToon);
                materialEditor.TexturePropertySingleLine(new GUIContent("カスタム影色"),pShadeColor);
                materialEditor.ColorProperty(pShadeColor0,"薄影 色");
                materialEditor.RangeProperty(pShadeShift0,"薄影 位置");
                materialEditor.RangeProperty(pShadeToony0,"薄影 ぼかし");
                materialEditor.ColorProperty(pShadeColor1,"濃影 色");
                materialEditor.RangeProperty(pShadeShift1,"濃影 位置");
                materialEditor.RangeProperty(pShadeToony1,"濃影 ぼかし");
                materialEditor.TexturePropertySingleLine(new GUIContent("影にテクスチャをブレンド"),pShadowTex,pShadowTextureLevel);
            }}

            fresnelFactor = Foldout("マットキャップとリムライト",fresnelFactor);
            if(fresnelFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.TexturePropertySingleLine(new GUIContent("マスク"),pFresnelMask);
                EditorGUILayout.LabelField("リムライト", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                    materialEditor.ColorProperty(pRimColor,"リムカラー");
                    materialEditor.RangeProperty(pRimLift,"リムシフト");
                    materialEditor.RangeProperty(pRimPower,"リムパワー");
                    materialEditor.RangeProperty(pRimFresnel,"フレネル乗数");
                    materialEditor.RangeProperty(pRimFresnelPower,"フレネルパワー");
                EditorGUI.indentLevel--;
                EditorGUILayout.LabelField("マットキャップ", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                    materialEditor.TexturePropertySingleLine(new GUIContent("マットキャップ"),pMatCap,pMatCapColor,pMatcapMode);
                EditorGUI.indentLevel--;
            }}

            outlineFactor = Foldout( "アウトライン", outlineFactor);
            if(outlineFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.ShaderProperty(pOutlineBase,"アウトラインモード");
                materialEditor.TexturePropertySingleLine(new GUIContent("マスク"), pOutlineMask);
                materialEditor.FloatProperty(pOutlineWidth,"太さ");
                materialEditor.TexturePropertySingleLine(new GUIContent("テクスチャ"),pOutlineTex,pOutlineColor);
                materialEditor.RangeProperty(pOutlineMainTexBlend,"テクスチャブレンド");
            }}

            emissionFactor = Foldout( "エミッション", emissionFactor);
            if(emissionFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.TexturePropertySingleLine(new GUIContent("マスク"), pEmissionMask);
                materialEditor.TexturePropertyWithHDRColor(new GUIContent("エミッション"), pEmissionTex, pEmissionColor, new ColorPickerHDRConfig(0, 100, 0.01f, 3.0f), false);
                materialEditor.TextureScaleOffsetProperty(pEmissionTex);
                materialEditor.VectorProperty(pEmissionVelocity,"エミッシブスクロール(wは速度)");
                materialEditor.VectorProperty(pEmissionScroll,"テクスチャスクロール");
                materialEditor.RangeProperty(pEmissionWidth,"太さ");
            }}
            
            wireframeFactor = Foldout( "ワイヤーフレーム", wireframeFactor);
            if(wireframeFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.ColorProperty(pWireframeColor,"色");
                materialEditor.RangeProperty(pWireframeBlend,"テクスチャブレンド");
                materialEditor.RangeProperty(pWireframeWidth,"太さ");
            }}

            parallaxFactor = Foldout( "パララックスマッピング", parallaxFactor);
            if(parallaxFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.ShaderProperty(pAlpha2Parallax,"透明度をパララックスにする");
                materialEditor.TexturePropertySingleLine(new GUIContent("マスク"),pParallaxMask);
                materialEditor.TexturePropertySingleLine(new GUIContent("パララックステクスチャ"),pParallaxTexture,pParallaxColor);
                materialEditor.VectorProperty(pParallaxScroll,"スクロール");
                materialEditor.FloatProperty(pParallaxDepth,"深さ");
                materialEditor.RangeProperty(pParallaxBlend,"ブレンド");
                materialEditor.RangeProperty(pParallaxAdd,"足し算");
                materialEditor.RangeProperty(pParallaxMul,"掛け算");
            }}

            geometryFactor = Foldout( "頂点シェーダー", geometryFactor);
            if(geometryFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.TexturePropertySingleLine(new GUIContent("メモリ"), pPositionMemory,pPositionPower);
                materialEditor.TextureScaleOffsetProperty(pPositionMemory);
                materialEditor.VectorProperty(pMemoryScroll,"スクロール");
                materialEditor.RangeProperty(pPositionFactor,"ポリゴンベースか頂点ベースか");
                materialEditor.RangeProperty(pExtrusionFactor,"爆発");
                materialEditor.RangeProperty(pExtrusionNoise,"爆発ノイズ");
                materialEditor.VectorProperty(pGrabity,"重力");
                materialEditor.RangeProperty(pRotationFactor,"回転");
                materialEditor.RangeProperty(pRotationNoise,"回転ノイズ");
                materialEditor.RangeProperty(pScaleFactor,"スケール");
                materialEditor.RangeProperty(pScaleNoise,"スケールノイズ");

                doppelgengerFactor = Foldout( "分身", doppelgengerFactor, EditorGUI.indentLevel);
                if(doppelgengerFactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(pIS_DOPPELGENGER,"ドッペルゲンガー");
                    if(pIS_DOPPELGENGER.floatValue==1){
                        materialEditor.ShaderProperty(pDoppelganger,"分身数");
                        materialEditor.VectorProperty(pDoppelgangerPos,"分身方向");
                    }
                    EditorGUI.indentLevel--;
                }

                gpupFactor = Foldout( "GPUパーティクル", gpupFactor, EditorGUI.indentLevel);
                if(gpupFactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.RangeProperty(pParticleFactor,"パーティクルファクター");
                    materialEditor.ColorProperty(pParticleColor,"カラー");
                    materialEditor.RangeProperty(pParticleColorFactor,"テクスチャブレンド");
                    materialEditor.RangeProperty(pParticleSize,"サイズ");
                    if(pNOISE_GENERATE.floatValue==1) materialEditor.RangeProperty(pPositionNoise,"ノイズ");
                    materialEditor.RangeProperty(pObjPosFactor,"オブジェクトポジションファクター");
                    materialEditor.RangeProperty(pObjPosNoise,"ファクターノイズ");
                    EditorGUI.indentLevel--;
                }
                matrixFactor = Foldout( "変換行列", matrixFactor, EditorGUI.indentLevel);
                if(matrixFactor)
                {
                    EditorGUI.indentLevel++;
                    materialEditor.ShaderProperty(pModel2World,"ワールド変換行列");
                        EditorGUI.indentLevel++;
                        materialEditor.VectorProperty(pModelRot,"回転");
                        materialEditor.VectorProperty(pModelScale,"拡大");
                        materialEditor.VectorProperty(pModelPos,"移動");
                        EditorGUI.indentLevel--;
                    materialEditor.ShaderProperty(pWorld2View,"ビュー変換行列");
                        EditorGUI.indentLevel++;
                        materialEditor.VectorProperty(pCameraRot,"回転");
                        materialEditor.VectorProperty(pCameraScale,"拡大");
                        materialEditor.VectorProperty(pCameraPos,"移動");
                        EditorGUI.indentLevel--;
                    materialEditor.ShaderProperty(pBillBoard,"ビルボード行列");
                    if(pBillBoard.floatValue==1){
                        EditorGUI.indentLevel++;
                        materialEditor.VectorProperty(pBillBoardRot,"回転");
                        materialEditor.VectorProperty(pBillBoardScale,"拡大");
                        materialEditor.VectorProperty(pBillBoardPos,"移動");
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                }
            }}

            fakebloomFactor = Foldout( "フェイクブルーム", fakebloomFactor);
            if(fakebloomFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.ShaderProperty(pBloomZWrite,"ZWrite");
                //materialEditor.ShaderProperty(pBloomDstBlend,"DstBlend");
                materialEditor.ShaderProperty(pBloomBase,"ベース");

                //materialEditor.ShaderProperty(pBloomDstBlend,"ブレンドモード");
                materialEditor.TexturePropertySingleLine(new GUIContent("マスク"),pBloomMask);
                materialEditor.ShaderProperty(pFurSaturate,"メインテクスチャノーマライズ");
                materialEditor.ShaderProperty(pFurNoise,"ノイズ");

                materialEditor.ShaderProperty(pBloomCullMode,"カリングモード");
                BloomBlendMode bb_mode = (BloomBlendMode)this.bloomBlendProp.floatValue;
                EditorGUI.BeginChangeCheck();
                bb_mode = (BloomBlendMode)EditorGUILayout.Popup("モード", (int)bb_mode, Enum.GetNames(typeof(BloomBlendMode)));
                if (EditorGUI.EndChangeCheck()) {
                    this.bloomBlendProp.floatValue = (float)bb_mode;
                    foreach (UnityEngine.Object obj in this.bloomBlendProp.targets)
                        this.SetupBloomBlendMode(obj as Material, bb_mode);
                }
                
                materialEditor.TexturePropertyWithHDRColor(new GUIContent("テクスチャ"), pBloomTex, pBloomCol, new ColorPickerHDRConfig(0, 100, 0.01f, 3.0f), false);
                materialEditor.TextureScaleOffsetProperty(pBloomTex);
                materialEditor.RangeProperty(pBloomMainTexBlend,"テクスチャブレンド");
                materialEditor.RangeProperty(pBloomRange,"長さ");
                materialEditor.RangeProperty(pBloomBrightness,"明るさ");
                materialEditor.RangeProperty(pBloomKnee,"Knee");
                materialEditor.RangeProperty(pBloomSoftknee,"SoftKnee");
                materialEditor.RangeProperty(pBloomThreshold,"Threshold");
                materialEditor.RangeProperty(pFurWindPower,"風の強さ");
                materialEditor.RangeProperty(pFurWindSpeed,"風のスピード");
                materialEditor.RangeProperty(pFurWindType,"風のタイプ");
                materialEditor.VectorProperty(pFurGravity,"重力");
                materialEditor.RangeProperty(pBloomLightEffect,"ブルームパスへのライトの影響");
            }}

            noiseGeneratoFactor = Foldout( "ノイズ", noiseGeneratoFactor);
            if(noiseGeneratoFactor){using (new EditorGUILayout.VerticalScope("box"))
            {
                materialEditor.ShaderProperty(pNOISE_GENERATE,"ノイズ生成(超重い)");
                materialEditor.ShaderProperty(pFBM,"FBMノイズ");
                materialEditor.ShaderProperty(pNoiseMode,"ノイズモード");
                materialEditor.VectorProperty(pNoise_ST,"ノイズスケール");
                materialEditor.VectorProperty(pNoiseScroll,"スクロール");
            }}
        }

        settingFactor = Foldout( "レガシーセッティング",settingFactor);
        if(settingFactor)
        {
            materialEditor.PropertiesDefaultGUI(properties);
        }
    
    }
	private void SetupBlendMode(Material material, BlendMode blendMode, StencilMode stencilMode) {
        switch (blendMode) {
            case BlendMode.Opaque:
                material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
                material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);
                material.SetFloat("_ZWrite", (float)1);
                material.renderQueue = 2000;
                break;
            case BlendMode.Transparent:
                material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                material.SetFloat("_ZWrite", (float)1);
                material.renderQueue = 3000;
                break;
            case BlendMode.Additive:
                material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
                material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.One);
                material.SetFloat("_ZWrite", (float)0);
                material.renderQueue = 3100;
                break;
            case BlendMode.AdditiveTransparent:
                material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.One);
                material.SetFloat("_ZWrite", (float)0);
                material.renderQueue = 3100;
                break;
        }
        switch(stencilMode){
            case StencilMode.Keep :
                material.renderQueue += 0;
                break;
            case StencilMode.Mask :
                material.renderQueue += -1;
                break;
            case StencilMode.Out :
                material.renderQueue += 1;
                break;
        }
        if(AutoShaderChange){
            Shader sOpaque = Shader.Find("HOTATE/GottaniToon");
            Shader sOpaqueStencil = Shader.Find("Hidden/HOTATE/GottaniToon_Stencil");
            Shader sTransparent = Shader.Find("Hidden/HOTATE/GottaniToon_Transparent");
            Shader sTransparentStencil = Shader.Find("Hidden/HOTATE/GottaniToon_Transparent_Stencil");
            if(material.renderQueue <= 2000){
                material.shader = sOpaque;
            }else if(material.renderQueue <= 2500){
                material.shader = sOpaqueStencil;
            }else if(material.renderQueue <= 3000){
                material.shader = sTransparent;
            }else{
                material.shader = sTransparentStencil;
            }
        }
    }

    private void SetupStencilMode(Material material, BlendMode blendMode, StencilMode stencilMode) {
        switch(blendMode){
            case BlendMode.Opaque:
                material.renderQueue = 2000;
                break;
            case BlendMode.Transparent:
                material.renderQueue = 3000;
                break;
            case BlendMode.Additive:
                material.renderQueue = 3100;
                break;
            case BlendMode.AdditiveTransparent:
                material.renderQueue = 3100;
                break;
        }
        switch(stencilMode){
            case StencilMode.Keep :
                material.SetFloat("_StencilComp", (float)UnityEngine.Rendering.CompareFunction.Always);
                material.SetFloat("_StencilPass", (float)UnityEngine.Rendering.StencilOp.Keep);
                material.renderQueue += 0;
                break;
            case StencilMode.Mask :
                material.SetFloat("_StencilComp", (float)UnityEngine.Rendering.CompareFunction.Always);
                material.SetFloat("_StencilPass", (float)UnityEngine.Rendering.StencilOp.Replace);
                material.renderQueue += -1;
                break;
            case StencilMode.Out :
                material.SetFloat("_StencilComp", (float)UnityEngine.Rendering.CompareFunction.NotEqual);
                material.SetFloat("_StencilPass", (float)UnityEngine.Rendering.StencilOp.Keep);
                material.renderQueue += 1;
                break;
            case StencilMode.In :
                material.SetFloat("_StencilComp", (float)UnityEngine.Rendering.CompareFunction.Equal);
                material.SetFloat("_StencilPass", (float)UnityEngine.Rendering.StencilOp.Keep);
                material.renderQueue += 1;
                break;
        }
        if(AutoShaderChange){
            Shader sOpaque = Shader.Find("HOTATE/GottaniToon");
            Shader sOpaqueStencil = Shader.Find("Hidden/HOTATE/GottaniToon_Stencil");
            Shader sTransparent = Shader.Find("Hidden/HOTATE/GottaniToon_Transparent");
            Shader sTransparentStencil = Shader.Find("Hidden/HOTATE/GottaniToon_Transparent_Stencil");
            if(material.renderQueue <= 2000){
                material.shader = sOpaque;
            }else if(material.renderQueue <= 2500){
                material.shader = sOpaqueStencil;
            }else if(material.renderQueue <= 3000){
                material.shader = sTransparent;
            }else{
                material.shader = sTransparentStencil;
            }
        }
    }
    private void SetupBloomBlendMode(Material material, BloomBlendMode blendMode) {
        switch(blendMode){
            case BloomBlendMode.Bloom :
                material.SetFloat("_BloomSrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
                material.SetFloat("_BloomDstBlend", (float)UnityEngine.Rendering.BlendMode.One);
                break;
            case BloomBlendMode.HardFur :
                material.SetFloat("_BloomSrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
                material.SetFloat("_BloomDstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;
            case BloomBlendMode.SoftFur :
                material.SetFloat("_BloomSrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
                material.SetFloat("_BloomDstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                break;
        }
    }
     private static Texture2D TextureField(string name, Texture2D texture)
    {
        GUILayout.BeginVertical();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.UpperCenter;
        style.fixedWidth = 70;
        GUILayout.Label(name, style);
        var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
        GUILayout.EndVertical();
        return result;
    }
    public static Texture2D ToTexture2D(Texture self )
    {
        var sw = self.width;
        var sh = self.height;
        var format = TextureFormat.RGBA32;
        var result = new Texture2D( sw, sh, format, false );
        var currentRT = RenderTexture.active;
        var rt = new RenderTexture( sw, sh, 32 );
        Graphics.Blit( self, rt );
        RenderTexture.active = rt;
        var source = new Rect( 0, 0, rt.width, rt.height );
        result.ReadPixels( source, 0, 0 );
        result.Apply();
        RenderTexture.active = currentRT;
        return result;
    }
    
      static bool Foldout(string title,bool display,float shift)
      {
          var style = new GUIStyle();
          style.font = new GUIStyle(EditorStyles.boldLabel).font;
          style.border = new RectOffset(15, 7, 4, 4);
          style.fixedHeight = 22;
          style.contentOffset = new Vector2(20f +shift*10f, 2f);

          var rect = GUILayoutUtility.GetRect(16f, 22f, style);
          GUI.Box(rect, title, style);

          var e = Event.current;

          var toggleRect = new Rect(rect.x + 4f +shift*10f, rect.y + 2f, 13f, 13f);
          if (e.type == EventType.Repaint)
          {
              EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
          }

          if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
          {
              display = !display;
              e.Use();
          }

          return display;
      }
      static bool Foldout(string title,bool display)
      {
          var style = new GUIStyle("ShurikenModuleTitle");
          style.font = new GUIStyle(EditorStyles.boldLabel).font;
          style.border = new RectOffset(15, 7, 4, 4);
          style.fixedHeight = 22;
          style.contentOffset = new Vector2(20f , -2f);

          var rect = GUILayoutUtility.GetRect(16f, 22f, style);
          GUI.Box(rect, title, style);

          var e = Event.current;

          var toggleRect = new Rect(rect.x + 4f , rect.y + 2f, 13f, 13f);
          if (e.type == EventType.Repaint)
          {
              EditorStyles.foldout.Draw(toggleRect, false, false, display, false);
          }

          if (e.type == EventType.MouseDown && rect.Contains(e.mousePosition))
          {
              display = !display;
              e.Use();
          }

          return display;
      }
}