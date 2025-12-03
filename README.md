# **UserProfileBDNR – Subsistema de Gestión de Usuarios y Perfiles**

Este proyecto implementa un insert y get del subsistema de gestión de usuarios y perfiles mediante una **WebAPI en .NET 8**, utilizando **MongoDB** como base documental y **Redis** como caché en memoria.  
Redis se ejecuta mediante **Docker Desktop**.

Desarrollado en **Visual Studio 2022**.

---

## **Tecnologías utilizadas**

- .NET 8.0  
- ASP.NET Core WebAPI  
- MongoDB (instalado localmente)  
- MongoDB Compass (IDE)  
- MongoDB Driver 3.5.2  
- Redis 2.10.1 (contenedor Docker)  
- Docker Desktop  
- Visual Studio 2022  

---

## **Requisitos previos**

Asegurate de tener instalados:

---

### **1. .NET 8 SDK**

Descargar:  
https://dotnet.microsoft.com/download

Verificar instalación:

```bash
dotnet --version
```

---

### **2. Visual Studio 2022**

Workloads recomendados:

- ASP.NET and web development  
- .NET desktop development  

---

### **3. MongoDB (instalación local)**

Descargar MongoDB Community Server:  
https://www.mongodb.com/try/download/community

Instalar con:

- **MongoDB como servicio de Windows (recomendado).**

Verificar el servicio:

```bash
net start MongoDB
```

Instalar MongoDB Compass:  
https://www.mongodb.com/products/compass

---

### **4. Docker Desktop (para ejecutar Redis)**

Descargar Docker Desktop:  
https://www.docker.com/products/docker-desktop/

Una vez instalado y ejecutándose:

Levantar Redis:

```bash
docker run --name redis -p 6379:6379 -d redis
```

Verificar contenedor:

```bash
docker ps
```

Redis quedará accesible en:

```
localhost:6379
```

---

---

## **Endpoints desarrollados**

Definir `{{baseurl}}` según el puerto en que se ejecute el proyecto.

### **Crear un usuario**
**POST:**  
```
{{baseurl}}/api/users/create
```

### **Obtener un usuario**
**GET:**  
```
{{baseurl}}/api/users/{{UserId}}
```

### **Obtener resumen para recomendador**
**GET:**  
```
{{baseurl}}/api/users/{{UserId}}/summary-for-recommender
```

---

## **Ejemplo de JSON para creación de usuario**

```json
{
  "username": "Martin",
  "email": "Martin@example.com",
  "twoFactorEnabled": true,

  "privacy": {
    "showProfile": true,
    "showFollowers": true,
    "showActivity": true
  },

  "notifications": {
    "emailNotifications": true,
    "pushNotifications": false,
    "extra": {
      "sound": "high",
      "frequency": "daily"
    }
  },

  "accountExtra": {
    "signupMethod": "email",
    "referralCode": "ABCD1234",
    "deviceInfo": {
      "os": "android",
      "version": "14",
      "model": "Samsung S23"
    }
  },

  "displayName": "Martin",
  "photoUrl": null,
  "languages": [
    { "code": "en", "level": 12, "xp": 15230, "active": true }
  ],

  "streak": { "currentStreak": 45, "maxStreak": 50 },
  "totalXP": 15230,
  "achievements": ["first_lesson", "10_day_streak"],
  "friends": ["user_1", "user_2"],
  "followers": ["user_3"],

  "plusStatus": {
    "isActive": true,
    "expiration": "2026-01-01T00:00:00Z",
    "extra": {
      "plan": "family",
      "renewal": "auto"
    }
  },

  "profileExtra": {
    "preferences": {
      "favorite_topics": ["grammar", "stories", "vocabulary"],
      "difficulty": "medium",
      "content_tags": [
        { "id": "stories", "weight": 0.9, "source": "explicit" }
      ]
    },
    "security": {
      "loginAlerts": true,
      "lastLoginIp": "10.0.0.5"
    },
    "uiSettings": {
      "theme": "dark",
      "fontSize": 16
    }
  }
}
```
---

## **Notas importantes**

- MongoDB debe estar corriendo como servicio antes de ejecutar la API.  
- Docker Desktop debe estar abierto para poder levantar Redis.  
- Redis mejora la performance mediante cacheo de usuarios, pero la API funciona aunque Redis no esté disponible.  
