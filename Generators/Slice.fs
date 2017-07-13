/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Slice generator for CodeControl",
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
            "NAME": "clockwise",
            "TYPE": "bool"
        }
    ] }*/

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

void main() {
    vec4 aspect = getAspect();
    
    vec2 v1 = vec2(x1, y1);
    vec2 v2 = vec2(x2, y2);
    
    vec3 color = vec3(slice(aspect, v1, v2, clockwise));
    
    gl_FragColor = vec4(color, 1.0);
}