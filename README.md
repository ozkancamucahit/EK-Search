
# web scraping and elastic search

UI side uses javascript for scraping. Multi thread requests download information and generate json doc for ElasticSearch


**Prerequisites**


- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Node.js](https://nodejs.org/en)
- [npm](https://www.npmjs.com/) (Node Package Manager)

**Cloning the Repository**

```bash
git clone https://github.com/ozkancamucahit/EK-Search.git
cd Ek-Search
```

**Installation**

Install the project dependencies using npm:

```bash
cd Scraper
npm i
```
Run scraping code which uses JavaScript

```bash
npm start
```

Start docker desktop to run following command to launch ElasticSearch and Kibana. This will run docker-compose.yml file

```bash
docker-compose up -d 
```


Open [http://localhost:9200](http://localhost:9200) in your browser to see ElasticSearch

Open [http://localhost:5601](http://localhost:5601) in your browser to see Kibana

Launch Razor pages app to search 