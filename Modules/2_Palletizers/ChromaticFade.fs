/*{
    "ISFVSN": "2.0",
    "CREDIT": "VJ Codec",
    "DESCRIPTION": "Chromatic fade palletizer for CodeControl",
	"CATEGORIES": [
        "palletizer",
        "CodeControl"
	],
	"INPUTS": [
		{
			"NAME": "image",
			"TYPE": "image"
		},
		{
			"NAME": "hue_1",
			"TYPE": "float"
		},
		{
			"NAME": "sat_1",
			"TYPE": "float"
		},
		{
			"NAME": "val_1",
			"TYPE": "float"
		},
		{
			"NAME": "hue_2",
			"TYPE": "float"
		},
		{
			"NAME": "sat_2",
			"TYPE": "float"
		},
		{
			"NAME": "val_2",
			"TYPE": "float"
		},
		{
			"NAME": "hue_curve",
			"TYPE": "float"
		},
		{
			"NAME": "sat_curve",
			"TYPE": "float"
		},
		{
			"NAME": "val_curve",
			"TYPE": "float"
		}
	]
}*/
vec3 rgb2hsv(vec3 c)
{
    vec4 K = vec4(0.0, -0.333333333, 0.666666667, -1.0);
    vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
    vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}
vec3 hsv2rgb(vec3 c) {
    vec4 K = vec4(1.0, 0.666666667, 0.333333333, 3.0);
    vec3 p = abs(fract(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * mix(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
}

void main() {
	vec4 pixel = IMG_THIS_PIXEL(image);
	vec3 hsv1 = vec3(hue_1, sat_1, val_1);
	vec3 hsv2 = vec3(hue_2, sat_2, val_2);
	vec3 curve = vec3(hue_curve, sat_curve, val_curve);

	vec3 indices = pow(vec3(pixel.a), curve);
	vec3 color = hsv2rgb(mix(hsv1, hsv2, indices));
	gl_FragColor = vec4(color, 1.0);
}