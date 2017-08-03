/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Line segment fade generator for CodeControl",
    "CATEGORIES": [
        "generator",
        "CodeControl"
    ],
    "INPUTS": [
        {
            "NAME": "x1",
            "TYPE": "float"
        },
        {
            "NAME": "y1",
            "TYPE": "float"
        },
        {
            "NAME": "x2",
            "TYPE": "float"
        },
        {
            "NAME": "y2",
            "TYPE": "float"
        },
        {
            "NAME": "thickness",
            "TYPE": "float",
            "MIN": 0.0
        },
        {
            "NAME": "intensity",
            "TYPE": "float",
            "MIN": 0.0
        },
        {
            "NAME": "fade_out",
            "TYPE": "bool",
            "DEFAULT": true
        },
        {
            "NAME": "clockwise",
            "TYPE": "bool"
        }
    ]
}*/

vec4 getAspect() {
    vec4 aspect;
    aspect.z = max(1.0, RENDERSIZE.x/RENDERSIZE.y);
    aspect.w = max(1.0, RENDERSIZE.y/RENDERSIZE.x);
    aspect.xy = aspect.zw * isf_FragNormCoord.xy;
    return aspect;
}
float slice(vec4 aspect, vec2 p1, vec2 p2, bool clockwise) {
    vec2 points = p2 - p1;
    vec2 pix = aspect.yx - p1.yx;
    pix.x = -pix.x;
    float inverter = 2.0*float(clockwise) - 1.0;
    return float(p1 != p2)*step(0.0, inverter*dot(points, pix));
}
vec4 fadeEndpoint(vec4 aspect, vec2 p1, vec2 p2, float thickness, bool fadeOut, float intensity) {
    float len = distance(aspect.xy, p1);
    bool side = dot(p2 - p1, aspect.xy - p1) > 0.0;
    float include = float(len <= thickness || side);
    float fadeDist = len/thickness;
    fadeDist = float(fade_out)*fadeDist + (1.0 - float(fade_out))*(1.0 - fadeDist);
    vec4 color = vec4(include);
    color.a = max(float(side), 1.0 - pow(fadeDist, intensity));
    return color;
}
vec4 fadeLine(vec4 aspect, vec2 p1, vec2 p2, float thickness, float intensity, bool fadeOut, bool clockwise) {
    if (thickness == 0.0 || intensity == 0.0)
        return vec4(0.0);
    else {
        vec2 v1 = p2 - p1;
        vec2 v2 = aspect.xy - p1;
        float orthoLength = length(v2 - dot(v1, v2)/dot(v1, v1)*v1);
        float fadeDist = orthoLength/thickness;
        fadeDist = float(fade_out)*fadeDist + (1.0 - float(fade_out))*(1.0 - fadeDist);
        vec4 color = vec4(float(orthoLength < thickness));
        color.rgb *= slice(aspect, p1, p2, clockwise);
        color.a = color.r * max(0.0, 1.0 - pow(fadeDist, intensity));
        return color;
    }
}
vec4 fadeLineSegment(vec4 aspect, vec2 p1, vec2 p2, float thickness, float intensity, bool fadeOut, bool clockwise) {
    vec4 ep1 = fadeEndpoint(aspect, p1, p2, thickness, fadeOut, intensity);
    vec4 ep2 = fadeEndpoint(aspect, p2, p1, thickness, fadeOut, intensity);
    vec4 l = fadeLine(aspect, p1, p2, thickness, intensity, fadeOut, clockwise);

    vec4 color;
    color.rgb = ep1.rgb*ep2.rgb*l.rgb;
    color.a = min(ep1.a, min(ep2.a, l.a));
    return color;
}

void main() {
    vec4 aspect = getAspect();

    vec2 v1 = vec2(x1, y1);
    vec2 v2 = vec2(x2, y2);

    vec4 color = fadeLineSegment(aspect, v1, v2, thickness, intensity, fade_out, clockwise);

    gl_FragColor = color;
}
