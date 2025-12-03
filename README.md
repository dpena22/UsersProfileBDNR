# **UserProfileBDNR – Subsistema de Gestión de Usuarios y Perfiles**

Este proyecto implementa un subsistema de gestión de usuarios y perfiles mediante una **WebAPI en .NET 8**, utilizando **MongoDB** como base documental y **Redis** como caché en memoria.  
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

## **Notas importantes**

- MongoDB debe estar corriendo como servicio antes de ejecutar la API.  
- Docker Desktop debe estar abierto para poder levantar Redis.  
- Redis mejora la performance mediante cacheo de usuarios, pero la API funciona aunque Redis no esté disponible.  
