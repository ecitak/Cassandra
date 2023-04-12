# Cassandra
CRUD Operations in Cassandra NoSQL Database.
This is the official source code repository for Apache Cassandra database. Apache Cassandra is a highly scalable, high-performance, and distributed NoSQL database. Its high scalability and high availability of data make it ideal for cloud applications.

# Properties
Apache Cassandra comes with many features:

Flexible data model: It uses a key-value model with data groups called column families, which provides a more flexible data model compared to relational databases.

High performance: Apache Cassandra is optimized for storing and processing data in large-scale and high-density environments.

High scalability: Apache Cassandra automatically handles data distribution among nodes of the database cluster, increasing the system's horizontal scalability.

High availability: Apache Cassandra offers multi-node replication to prevent data loss, helping to maintain high availability.

# Installation
Please follow the link for installation details. https://phoenixnap.com/kb/install-cassandra-on-windows

# Usage
```
var user = new User
{
    Id = Guid.NewGuid(),
    Name = "Emir",
    LastName = "Çitak",
};

ICassandraRepository<User> repository = new CassandraRepository<User>
                                            ("demo");

//Insert
await repository
      .AddAsync(user);

//Get All
var users = await repository
                  .GetAllAsync();
```

