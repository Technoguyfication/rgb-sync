# rgb-sync

I have:
* NZXT Kraken X41 Cooler
* Razer Deathadder Chroma Mouse
* Razer Deathstalker Keyboard
* Some LED strips inside my Phanteks P400S case

... that I want to all be in sync RGB-wise.

My plan is to use an Arduino connected to a USB 2.0 header inside my computer for case lighting control, with the rgb strips connected to the GPIO pins using a transistor. It will get power from the USB header / molex connector and communicate through the host software using the USB serial connection.

If possible, I want to sync the NZXT logo on my cooler with the RGB as well, but I'm not sure how possible this will be without writing custom drivers.

The program will receive RGB data from Razer Synapse, and relay it to all RGB devices. Maybe in the future I'll add integration for other RGB stuff if I buy anything else, including IoT devices.

I'm writing this for myself but feel free to use/adapt it to your own purposes. This repository uses the [WTFPL](http://www.wtfpl.net/about/) license which means you can literally do whatever you want to/with this code. I don't care.
