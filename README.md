# Demo c# with Dapper and MySQL



## Run project

### With docker-compose

#### Build docker-compose
```bash
docker-compose build
```

#### Start project
```bash
docker-compose up -d
```

#### Stop project
```bash
docker-compose down
```



### Manually run

#### Create docker network
```bash
docker network create --driver=bridge demos
```

#### Run MySQL container
[Docker image](https://hub.docker.com/_/mysql)
```bash
docker run -d --name demo-mysql --net demos -e MYSQL_ALLOW_EMPTY_PASSWORD=yes -p 3306:3306 -v $(pwd)/data/database-init.sql:/docker-entrypoint-initdb.d/database-init.sql mysql:8.0.27
```
* `-e MYSQL_ALLOW_EMPTY_PASSWORD=yes` to start container without password.
* `-p 3306:3306` to expose port 3306 to host machine.
* `-v ./data/database-init.sql:/docker-entrypoint-initdb.d/database-init.sql` to create the database when a container is started for the first time.


#### Run App container

##### Build image
```bash
docker build --force-rm -t demo-csharp-dapper-mysql .
```

##### Run App
```bash
docker run -d --net demos --name=demo-csharp-dapper-mysql -e DB_HOST=demo-mysql -p 8080:80 -t demo-csharp-dapper-mysql .
```