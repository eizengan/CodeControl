/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Faded triangle generator for CodeControl",
    "CATEGORIES": [
        "generator",
        "CodeControl"
    ],
    "INPUTS": [
        {
            "NAME": "x",
            "TYPE": "float"
        },
        {
            "NAME": "y",
            "TYPE": "float"
        },
        {
            "NAME": "width",
            "TYPE": "float",
            "MIN": 0.0
        },
        {
            "NAME": "height",
            "TYPE": "float",
            "MIN": 0.0
        },
        {
            "NAME": "vertex_x",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 0.0
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
vec4 fadeEndpoint(vec4 aspect, vec2 p1, vec2 p2, float thickness, float intensity) {
    float len = distance(aspect.xy, p1);
    bool side = dot(p2 - p1, aspect.xy - p1) > 0.0;
    vec4 color = vec4(float(len <= thickness || side));
    color.a = max(float(side), 1.0 - pow(len/thickness, intensity));
    return color;
}
vec4 fadeLine(vec4 aspect, vec2 p1, vec2 p2, float thickness, float intensity, bool clockwise) {
    if (thickness == 0.0 || intensity == 0.0)
        return vec4(0.0);
    vec2 v1 = p2 - p1;
    vec2 v2 = aspect.xy - p1;
    float orthoLength = length(v2 - dot(v1, v2)/dot(v1, v1)*v1);
    vec4 color = vec4(float(orthoLength < thickness));
    color.rgb *= slice(aspect, p1, p2, clockwise);
    color.a = color.r * max(0.0, 1.0 - pow(orthoLength/thickness, intensity));
    return color;
}
vec4 fadeLineSegment(vec4 aspect, vec2 p1, vec2 p2, float thickness, float intensity, bool clockwise) {
    vec4 ep1 = fadeEndpoint(aspect, p1, p2, thickness, intensity);
    vec4 ep2 = fadeEndpoint(aspect, p2, p1, thickness, intensity);

    vec4 color;
    color.rgb = ep1.rgb*ep2.rgb;
    color.a = min(ep1.a, ep2.a);
    if (p1 != p2) {
        vec4 l = fadeLine(aspect, p1, p2, thickness, intensity, clockwise);
        color.rgb *= l.rgb;
        color.a = min(color.a, l.a);
    }
    return color;
}

void main() {
    vec4 aspect = getAspect();

    vec2 v1 = vec2(x - 0.5*width, y - 0.5*height);
    vec2 v2 = vec2(x + 0.5*width, y - 0.5*height);
    vec2 v3 = vec2(x + (vertex_x - 0.5)*width, y + 0.5*height);

    vec4 l12 = fadeLineSegment(aspect, v1, v2, thickness, intensity, true);
    vec4 l23 = fadeLineSegment(aspect, v2, v3, thickness, intensity, true);
    vec4 l31 = fadeLineSegment(aspect, v3, v1, thickness, intensity, true);

    vec4 color;
    color.rgb = min(l12.rgb+l23.rgb+l31.rgb, 1.0);
    color.a = max(l12.a, max(l23.a, l31.a));

    gl_FragColor = color;
}
