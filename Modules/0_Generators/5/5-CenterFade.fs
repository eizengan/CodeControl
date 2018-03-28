/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Faded pentagon generator for CodeControl",
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
            "NAME": "base_width",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.5
        },
        {
            "NAME": "center_height",
            "TYPE": "float",
            "MIN": 0.0,
            "MAX": 1.0,
            "DEFAULT": 0.5
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
    vec4 color = vec4(max(float(side), 1.0 - pow(len/thickness, intensity)));
    color.a = float(len <= thickness || side);
    return color;
}
vec4 fadeLine(vec4 aspect, vec2 p1, vec2 p2, float thickness, float intensity, bool clockwise) {
    if (thickness == 0.0 || intensity == 0.0)
        return vec4(0.0);
    vec2 v1 = p2 - p1;
    vec2 v2 = aspect.xy - p1;
    float orthoLength = length(v2 - dot(v1, v2)/dot(v1, v1)*v1);
    vec4 color = vec4(max(0.0, 1.0 - pow(orthoLength/thickness, intensity)));
    color.a = float(orthoLength < thickness);
    color *= slice(aspect, p1, p2, clockwise);
    return color;
}
vec4 fadeLineSegment(vec4 aspect, vec2 p1, vec2 p2, float thickness, float intensity, bool clockwise) {
    vec4 ep1 = fadeEndpoint(aspect, p1, p2, thickness, intensity);
    vec4 ep2 = fadeEndpoint(aspect, p2, p1, thickness, intensity);

    vec4 color = ep1*ep2;
    if (p1 != p2) {
        vec4 l = fadeLine(aspect, p1, p2, thickness, intensity, clockwise);
        color = min(color, l);
    }
    return color;
}

void main() {
    vec4 aspect = getAspect();

    float base_y = y - 0.5*height;
    vec4 offsets;
    offsets.xz = vec2(0.5*width, height);
    offsets.yw = offsets.xz * vec2(base_width, center_height);

    vec2 v1 = vec2(x - offsets.y, base_y);
    vec2 v2 = vec2(x + offsets.y, base_y);
    vec2 v3 = vec2(x + offsets.x, base_y + offsets.w);
    vec2 v4 = vec2(x,             base_y + offsets.z);
    vec2 v5 = vec2(x - offsets.x, base_y + offsets.w);

    vec4 l12 = float(v1==v2)*vec4(0.0) + float(v1!=v2)*fadeLineSegment(aspect, v1, v2, thickness, intensity, true);
    vec4 l23 = float(v2==v3)*vec4(0.0) + float(v2!=v3)*fadeLineSegment(aspect, v2, v3, thickness, intensity, true);
    vec4 l34 = fadeLineSegment(aspect, v3, v4, thickness, intensity, true);
    vec4 l45 = fadeLineSegment(aspect, v4, v5, thickness, intensity, true);
    vec4 l51 = float(v5==v1)*vec4(0.0) + float(v5!=v1)*fadeLineSegment(aspect, v5, v1, thickness, intensity, true);

    vec4 color =  max(l12, max(l23, max(l34, max(l45, l51))));
	color.a = min(l12.a+l23.a+l34.a+l45.a+l51.a, 1.0);

    gl_FragColor = color;
}
