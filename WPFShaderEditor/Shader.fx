sampler2D input : register(s0);
float width : register(c0);
float height : register(c1);
float time : register(c2);
float4 DdxDdy : register(c6);

//static const float weights[5] = {0.2270270, 0.1945945, 0.1216216, 0.0540540, 0.0162162};
//static const float weights[5] = {0.0454054, 0.0389189, 0.02432432, 0.010810, 0.00324324};

/*static const float weights[11] = {
    0.1036861424,
    0.1002869063,
    0.09074334534,
    0.07681258349,
    0.06082708343,
    0.04506181171,
    0.03122966593,
    0.02024757718,
    0.01228077634,
    0.006968280361,
    0.003698898753,
};*/

static const float weights[11] = {
    1,
    0.9672161005,
    0.875173319,
    0.7408182207,
    0.5866462195,
    0.4345982085,
    0.3011942119,
    0.1952775628,
    0.118441829,
    0.06720551274,
    0.03567399335,
};

/*static const float weights[11] = {
    0.5,
    0.4836080502,
    0.4375866595,
    0.3704091103,
    0.2933231098,
    0.2172991043,
    0.150597106,
    0.09763878142,
    0.05922091451,
    0.03360275637,
    0.01783699667,
};*/

float4 lookup(float x, float y) : COLOR
{
    return x < -1 || x>1 || y < -1 || y>1 ? float4(0,0,0,1) : tex2D(input, float2(x / 2 + 0.5, y / 2 + 0.5));
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float x = (uv.x - 0.5) * 2;
    float y = (uv.y - 0.5) * 2;

    float4 color = tex2D(input, uv);

    float ddx = DdxDdy.x;
    float ddy = DdxDdy.w;

    color = float4(0,0,0,0);

    int radius = 10;
    float sum = 0;
    for (int i = -radius; i <= radius; i++) {
        float weight = weights[round(abs((float)i / radius * 10))];
        sum += weight;
        color += tex2D(input, float2(uv.x + ddx * i, uv.y)) * weight;
        color += tex2D(input, float2(uv.x, uv.y + ddy * i)) * weight;

        /*float4 colorH = float4(0,0,0,0);
        float sumH = 0;
        for (int h = -radius; h <= radius; h++){
            float weightH = weights[round(abs((float)h / radius * 10))];
            sumH += weightH;
            colorH += tex2D(input, float2(uv.x + ddx * h, uv.y + ddy * i)) * weightH;
        }
        colorH /= sumH;

        color += colorH * weight;*/
    }

    color /= sum;

    color /= 2;

    return color;
    //return color / 2;

    /*if (all(color == float4(0, 0, 0, 0))){
        return float4(1,0,1,1);
    }
    return float4(1,1,0,1);*/

    /*float pX = 50 * length(DdxDdy.xy);
    float pY = 50 * length(DdxDdy.zw);*/

    //multiply pixel and get uv coordinate
    /*float px = 50 * DdxDdy.x;
    float py = 50 * DdxDdy.w;

    if (uv.x<px&&uv.y<py){
        return float4(1,0,0,1);
    }
    if (uv.x>1-px&&uv.y<py){
        return float4(1,1,0,1);
    }
    if (uv.x<px&&uv.y>1-py){
        return float4(0,1,0,1);
    }
    if (uv.x>1-px&&uv.y>1-py){
        return float4(0,0,1,1);
    }
    return color;*/

    //return tex2D(input, float2(uv.x + sin((uv.y + time * 0.5) * 3.14159 * 10) * 0.1, uv.y));
    //return tex2D(input, uv);
    //return lookup(x,y * 2);
    /*float s, c;
    sincos(radians(time * 20), s, c);
    float t = x * s / c;
    float dis = sqrt(x * x + t * t) * sign(x) * sign(c);
    float r = 1 - abs(uv.x - 0.5);
    float a = dis + (-s * 0.3 + 0);
    float z = dis * pow(-c + 2, sign(x)) + 1;
    return lookup(sqrt(x * x + t * t) * sign(x) * sign(c), y * abs(z));*/
    /*float4 color = tex2D(input, uv);
    return color.a == 0 ? float4(1,0,0,1) : color;*/
}