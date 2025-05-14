#include "UnityCG.cginc"
sampler2D _MainTex;//自动获得前文定义的值
sampler2D _GrabTexture;//自动获得屏幕颜色。未渲染此物体之前
float4 _GrabTexture_TexelSize;//自动赋值。每个大约是1/1024，1024改为实际像素数
float4 _MainTex_ST;//自动获得

float2 Rect;//脚本设置，矩形宽度
float4 BorderC;//脚本设置，边框颜色
float BorderW;//脚本设置，边框宽度
float4 SelfColor;//脚本设置，本色
float Size;//脚本设置，模糊强度。默认强度应当为5
int Mix;//脚本设置，是否混合本色

struct In {
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
    float4 color : COLOR;
};
struct Out {
    float4 vertex : SV_POSITION;
    float2 uv : TEXCOORD0;
    float4 uvgrab : TEXCOORD1;
    float4 color : COLOR;
};
Out Transform(In v) {
    Out o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    #if UNITY_UV_STARTS_AT_TOP
        float scale = -1.0;
    #else
        float scale = 1.0;
    #endif
    o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y * scale) + o.vertex.w) * 0.5;//从-1到1 归入0到1
    o.uvgrab.zw = o.vertex.zw;
    o.color = v.color;
    return o;
}
float4 ClearAlpha(float4 X) {
    return float4(X.x, X.y, X.z, 1);
}
#define PI 3.141592
#define TAU (PI * 2.0)
float Gaussian(float d, float sigma) { //距离，最大半径
	return 1.0 / (sigma * sqrt(TAU)) * exp(-(d*d) / (2.0 * sigma * sigma));
}
float4 HBlur(Out i) : SV_Target {
    float4 color = float4(0.0, 0.0, 0.0, 0.0);
    float H = 0;
    for (int j = 0; j < 2*Size+1; j++) {
        float4 A = tex2D(_GrabTexture, i.uvgrab.xy + float2(0, (j-Size)*_GrabTexture_TexelSize.y));
        float G = Gaussian(abs(j-Size),Size);
        color += A * G;
        H += G;
    }
    return color/H;
}
float4 VBlurAndOutLine(Out i) : SV_Target {
    if (i.uv.x * Rect.x < BorderW || i.uv.y * Rect.y < BorderW || (1.0 - i.uv.x) * Rect.x < BorderW || (1.0 - i.uv.y) * Rect.y < BorderW){
        return BorderC;
    }
    float4 color = float4(0.0, 0.0, 0.0, 0.0);
    float H = 0;
    for (int j = 0; j < 2*Size+1; j++) {
        if(j>100)break;
        float4 A = tex2D(_GrabTexture, i.uvgrab.xy + float2((j-Size)*_GrabTexture_TexelSize.x, 0));
        if (Mix == 1) {
            A = SelfColor.w*SelfColor + (1 - SelfColor.w) * A;
            //从_MainTex采样并混合
            //A=lerp(tex2D(_MainTex, i.uv),A,A.w);
        }
        float G = Gaussian(abs(j-Size),Size);
        color += A * G;
        H += G;
    }
    return color/H;
}