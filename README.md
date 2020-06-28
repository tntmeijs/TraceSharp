# TraceSharp - a C# path-tracer
## About
This project is to demonstrate a wide variety of software engineering skills.
I've tried my best to show the kind of code I would write in a production environment, including unit tests.

The project itself isn't meant to be used as a production-grade renderer. It's just a fun and interesting way for me to show my skills.

## Resources
- Peter Shirley's "*Ray Tracing in a ___*" series
- Alan Wolfe's "*casual path tracing*" blog posts.

## Timeline (reverse chronological order)

### 28/06/2020 @ 14:25 - basic path tracing
Type | statistics
------------ | -------------
Resolution | 1280 x 720 pixels
Samples per pixel | 256
Bounces | 16
Threads | 1
Render time | 1043 seconds

![Basic path tracing](./Media/2_basic_path_tracing.png)

---

### 27/06/2020 @ 17:23 - simple sphere intersection
Type | statistics
------------ | -------------
Resolution | 1280 x 720 pixels
Samples per pixel | 1
Bounces | 0
Threads | 1
Render time | 14 seconds

![Sphere intersection](./Media/1_sphere_intersection.png)

---

### 26/06/2020 @ 23:20 - UV coordinates and ppm image output
Type | statistics
------------ | -------------
Resolution | 1280 x 720 pixels
Threads | 1
Render time | 14 seconds

![UV coordinates](./Media/0_uv_coordinates.png)
