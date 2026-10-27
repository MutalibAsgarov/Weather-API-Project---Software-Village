\# Weather API



\## Layihə haqqında



Bu layihə frontend hava tətbiqi üçün hazırlanmış API-dir. İstifadəçinin daxil etdiyi şəhərə görə cari hava məlumatlarını, hava proqnozunu və yağış ehtimalını OpenWeather API vasitəsilə əldə edir.



\## İstifadə olunan texnologiyalar



\- C#

\- ASP.NET Core Web API (.NET 10)

\- OpenWeather API

\- HttpClient

\- DotNetEnv

\- Swagger



\## Quraşdırma



Əvvəlcə layihəni GitHub-dan klonlayın:



```bash

git clone https://github.com/MutalibAsgarov/Weather-API-Project---Software-Village.git

```



Layihə qovluğuna keçin:



```bash

cd Weather-API-Project---Software-Village

```



Lazım olan paketləri bərpa edin:



```bash

dotnet restore

```



Layihənin `.csproj` faylının yerləşdiyi qovluqda `.env` adlı fayl yaradın və OpenWeather API açarını əlavə edin:



```env

OPENWEATHER\_API\_KEY=

```



Daha sonra layihəni başladın:



```bash

dotnet run

```



Qeyd: `.env` faylını GitHub-a yükləməyin. API açarını həmin faylda saxlayın.



\## Mühit dəyişənləri



Layihənin işləməsi üçün aşağıdakı mühit dəyişəni tələb olunur:



\- `OPENWEATHER\_API\_KEY` — OpenWeather API-dən hava məlumatlarını əldə etmək üçün istifadə olunur.



\## Verilənlər bazası



Bu layihədə verilənlər bazasından istifadə olunmur. Hava məlumatları OpenWeather API-dən alınır. Buna görə database yaratmağa və migration işlətməyə ehtiyac yoxdur.



\## API endpointləri



Bütün endpointlər `GET` metodundan istifadə edir. `city` parametrində hava məlumatları alınacaq şəhərin adı göndərilir.



\### 1. Cari hava məlumatları



\*\*URL:\*\* `/api/weather/current`



\*\*Method:\*\* `GET`



\*\*Request nümunəsi:\*\*



```http

GET /api/weather/current?city=Baku

```



\*\*Response nümunəsi:\*\*



```json

{

&#x20; "success": true,

&#x20; "data": {

&#x20;   "city": "Baku",

&#x20;   "country": "AZ",

&#x20;   "temperature": 18.03,

&#x20;   "realFeel": 17.32,

&#x20;   "windSpeed": 4.12,

&#x20;   "windDirection": "N",

&#x20;   "pressure": 1022,

&#x20;   "humidity": 68,

&#x20;   "sunrise": "06:42:00",

&#x20;   "sunset": "17:48:00",

&#x20;   "icon": "04d",

&#x20;   "description": "Clouds",

&#x20;   "dateTime": "2026-10-26T12:00:00"

&#x20; },

&#x20; "message": "OK"

}

```



\### 2. Hava proqnozu



\*\*URL:\*\* `/api/weather/forecast`



\*\*Method:\*\* `GET`



\*\*Request nümunəsi:\*\*



```http

GET /api/weather/forecast?city=Baku

```



\*\*Response nümunəsi:\*\*



```json

{

&#x20; "success": true,

&#x20; "data": \[

&#x20;   {

&#x20;     "dayName": "Monday",

&#x20;     "minTemp": 14.5,

&#x20;     "maxTemp": 20.8,

&#x20;     "icon": "04d",

&#x20;     "description": "Clouds"

&#x20;   },

&#x20;   {

&#x20;     "dayName": "Tuesday",

&#x20;     "minTemp": 13.2,

&#x20;     "maxTemp": 21.4,

&#x20;     "icon": "01d",

&#x20;     "description": "Clear"

&#x20;   }

&#x20; ],

&#x20; "message": "Forecast retrieved successfully"

}

```



\### 3. Yağış ehtimalı



\*\*URL:\*\* `/api/weather/rain-probability`



\*\*Method:\*\* `GET`



\*\*Request nümunəsi:\*\*



```http

GET /api/weather/rain-probability?city=Baku

```



\*\*Response nümunəsi:\*\*



```json

{

&#x20; "success": true,

&#x20; "data": \[

&#x20;   {

&#x20;     "timeLabel": "5PM",

&#x20;     "probability": 0,

&#x20;     "condition": "Clear"

&#x20;   },

&#x20;   {

&#x20;     "timeLabel": "6PM",

&#x20;     "probability": 20,

&#x20;     "condition": "Clouds"

&#x20;   }

&#x20; ],

&#x20; "message": "Rain probability retrieved successfully"

}

```



Qeyd: Response-lardakı hava məlumatları nümunə üçündür. Real nəticələr şəhərə və sorğunun göndərildiyi vaxta görə dəyişir. Yağış ehtimalı bəzi hallarda `null` ola bilər.



\### Xəta cavabları



API sorğu zamanı xəta baş verdikdə uyğun HTTP status kodu və mesaj qaytarır.



\- `400 Bad Request` — şəhər adı göndərilməyib.

\- `404 Not Found` — şəhər tapılmayıb.

\- `500 Internal Server Error` — server daxilində xəta baş verib.

\- `502 Bad Gateway` — cari hava məlumatları alınarkən xarici API ilə bağlı problem yaranıb.



\*\*Xəta cavabına nümunə:\*\*



```json

{

&#x20; "success": false,

&#x20; "data": null,

&#x20; "message": "City is required"

}

```



\## Swagger



Layihə işə salındıqdan sonra API endpointlərini Swagger vasitəsilə yoxlamaq mümkündür.



Swagger UI ünvanı:



```text

http://localhost:5214/swagger

```



Port fərqli olarsa, layihə başladıqda terminalda göstərilən ünvanı istifadə edin.



\## Müəllif



\*\*Mütalib Əsgərov\*\*



\- GitHub: https://github.com/MutalibAsgarov

\- LinkedIn: https://www.linkedin.com/in/mutalib-asgarov-3a532b284/

