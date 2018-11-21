# Delivery Service

## Stack

* **.NET Core 2.1**
* **C# Version 7.2**
* **Cassandra DB 3.11.2**

---

## Preview run

Before starting the solution please create the DB to run it.


### Authentication Api

First of all for you to be able to `Create`, `Update` and `Delete` the Point and/or Route you have to pull other project the <a href="https://github.com/marcelosf89/Delivery.Authentication" target="_blank">Delivery.Authentication</a>

---

### Cassandra

#### If you don't have cassandra Db you can pull the image in docker and run the container 

```
docker run -d -p 7000:7000 -p 9042:9042 -e CASSANDRA_USER=local -e CASSANDRA_PASSWORD=local --name local-cassandra cassandra:3.11.2
```

#### For access the cqlsh run this command in your command
```
docker exec -it local-cassandra cqlsh
```


#### After install the Cassandra db run this script to create the tables

1. Keyspace
```
CREATE KEYSPACE delivery WITH replication = {'class': 'SimpleStrategy', 'replication_factor': 1};
```

2. Enter in keyspace
```
USE delivery;
```

3. Create Tables
```
CREATE TABLE points (
  Code text,
  Description text,
  PRIMARY KEY (Code)
);

CREATE TABLE routes (
  PointFromCode text,
  PointToCode text,
  Cost double,
  Time double,
  PRIMARY KEY (PointFromCode, PointToCode)
);
```

4. Insert data
```
INSERT INTO points (Code, Description) VALUES ('a','a');
INSERT INTO points (Code, Description) VALUES ('b','b');
INSERT INTO points (Code, Description) VALUES ('c','c');
INSERT INTO points (Code, Description) VALUES ('d','d');
INSERT INTO points (Code, Description) VALUES ('e','e');
INSERT INTO points (Code, Description) VALUES ('f','f');
INSERT INTO points (Code, Description) VALUES ('g','g');
INSERT INTO points (Code, Description) VALUES ('h','h');
INSERT INTO routes (PointFromCode, PointToCode, Cost, Time) VALUES ('a','c', 20 , 1);
INSERT INTO routes (PointFromCode, PointToCode, Cost, Time) VALUES ('a','e', 5 , 30);
INSERT INTO routes (PointFromCode, PointToCode, Cost, Time) VALUES ('a','h', 1 , 10);
INSERT INTO routes (PointFromCode, PointToCode, Cost, Time) VALUES ('c','b', 12 , 1);
INSERT INTO routes (PointFromCode, PointToCode, Cost, Time) VALUES ('f','g', 50 , 40);
INSERT INTO routes (PointFromCode, PointToCode, Cost, Time) VALUES ('f','i', 50 , 45);
INSERT INTO routes (PointFromCode, PointToCode, Cost, Time) VALUES ('g','b', 73 , 64);
INSERT INTO routes (PointFromCode, PointToCode, Cost, Time) VALUES ('e','d', 5 , 3);
INSERT INTO routes (PointFromCode, PointToCode, Cost, Time) VALUES ('d','f', 50 , 4);
INSERT INTO routes (PointFromCode, PointToCode, Cost, Time) VALUES ('h','e', 1 , 30);
INSERT INTO routes (PointFromCode, PointToCode, Cost, Time) VALUES ('i','b', 5 , 65);
```

---

### Appsettings

* The Appsettings.json is configured with the default ports, if you want to change these configuration please open the Appsettings.json and change it.
 
```JSON
  "Auth": {
    "Host": "https://localhost:5002",
    "ApiName":  "delivery"
  },
  "CassandraConnection": {
    "Hosts": [ "localhost" ],
    "Keyspace": "delivery",
    "Port": 9042,
    "User": "local",
    "Password": "local"
  }
}

```

---

## Run Aplication

* The application is configured to show swagger only in `Dedug` configuration
** You can access the swagger with the URL `https://localhost:{port by default is 5001}/swagger`

* The application is configured to show api docs only in `Dedug` configuration
** You can access the api docs with the URL `https://localhost:{port by default is 5001}/api-docs`

### Authentication

### 1. Click in `Authorize` button

---

![Image of step 1](https://drive.google.com/uc?id=1ZzdiG1wURrGdG8em0FFABVWplgUrirMW&export=download)

---

### 2. On popup you write :
#### *  Fill the client_id: `delivery`
#### *  Check the scope `delivery`
#### *  Click in `Authorize`

---

![Image of step 2](https://drive.google.com/uc?id=1O9X18tmUTDK0c3HjzmlK2oGw10mji3fx&export=download)

---

### 3. After you been redirected to another page you have authentication
#### * Username: `admin`
#### * Password: `admin`

---

![Image of step 3](https://drive.google.com/uc?id=1LoO5fX8lX_uLV4IsoDMJxNpzkP5wMkcW&export=download)

--- 
 
### 4. In consent page:
#### * Check all checkboxes
#### * Click in `yes`

---

![Image of step4](https://drive.google.com/uc?id=1LoO5fX8lX_uLV4IsoDMJxNpzkP5wMkcW&export=download)

---

---

## Benchmark Report

### DeliveryController
| Method | origin | destination |     Mean |    Error |    StdDev | Allocated |
|------- |------- |------------ |---------:|---------:|----------:|----------:|
|  GetOk |      a |           b | 5.162 ms | 15.47 ms | 10.235 ms | 538.84 KB |
|  GetOk |      a |           f | 4.245 ms | 16.62 ms | 10.991 ms | 130.51 KB |
|  GetOk |      a |           i | 4.392 ms | 16.97 ms | 11.224 ms | 161.84 KB |
|  GetOk |      c |           f | 3.648 ms | 14.02 ms |  9.272 ms |   92.1 KB |
|  GetOk |      c |           i | 3.607 ms | 13.86 ms |  9.165 ms |   99.4 KB |
|  GetOk |      h |           b | 3.847 ms | 13.86 ms |  9.169 ms | 172.16 KB |
|  GetOk |      h |           f | 3.796 ms | 15.07 ms |  9.968 ms |  75.88 KB |
|  GetOk |      h |           i | 4.187 ms | 16.24 ms | 10.743 ms |  75.82 KB |

### PointsController
|                      Method |     Mean |     Error |    StdDev | Allocated |
|---------------------------- |---------:|----------:|----------:|----------:|
|                      SaveOk | 2.329 ms | 10.962 ms |  7.251 ms |       0 B |
| CreatePointPointEmailExists | 2.237 ms | 10.103 ms |  6.682 ms |       0 B |
|        DeletePointNotExists | 2.033 ms |  9.147 ms |  6.050 ms |       0 B |
|                    DeleteOk | 3.624 ms | 16.941 ms | 11.206 ms |       0 B |
|        UpdatePointNotExists | 2.627 ms | 11.877 ms |  7.856 ms |       0 B |
|                    UpdateOk | 2.336 ms | 10.889 ms |  7.202 ms |       0 B |
|      GetPointPointNotExists | 2.152 ms |  9.679 ms |  6.402 ms |       0 B |
|                  GetPointOk | 2.354 ms | 11.027 ms |  7.294 ms |       0 B |

