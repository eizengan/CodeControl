/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Faded rectangle generator for CodeControl",
    "CATEGORIES": [
        "generator",
        "CodeControl"
    ],
    "INPUTS": [
        {
            "NAME": "base_x",
            "TYPE": "float"
        },
        {
            "NAME": "base_y",
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

    vec4 offsets;
    offsets.xy = 0.5 * vec2(-width, width);                  //x offsets
    offsets.zw = 0.5 * vec2(thickness, height + thickness);  //y offsets

    vec2 v1 = vec2(base_x + offsets.x, base_y + offsets.z);
    vec2 v2 = vec2(base_x + offsets.x, base_y + offsets.w);
    vec2 v3 = vec2(base_x + offsets.y, base_y + offsets.w);
    vec2 v4 = vec2(base_x + offsets.y, base_y + offsets.z);

    vec4 l12 = fadeLineSegment(aspect, v1, v2, thickness, intensity, false);
    vec4 l23 = fadeLineSegment(aspect, v2, v3, thickness, intensity, false);
    vec4 l34 = fadeLineSegment(aspect, v3, v4, thickness, intensity, false);
    vec4 l41 = fadeLineSegment(aspect, v4, v1, thickness, intensity, false);

    vec4 color =  max(l12, max(l23, max(l34, l41)));
	color.a = min(l12.a+l23.a+l34.a+l41.a, 1.0);

    gl_FragColor = color;
}