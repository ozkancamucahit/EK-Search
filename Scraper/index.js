
import * as cheerio from 'cheerio';
import fs from 'fs';




const BASE_URL = 'https://www.sozcu.com.tr'

async function DowloadPage() {

  const response = await fetch(BASE_URL);
  const text = await response.text();
  let $ = cheerio.load(text);
  return $;
  
}

async function GetNewsCards(co){

  if(typeof(co) === 'undefined')
    return;

  const container = co('div.container')[1];
  const rows = co(container).find('div.news-card');
  return rows;
}

async function GetMansetNews(co){

  if(typeof(co) === 'undefined')
    return;

  const mansetSelector = co("div.swiper.swiper-manset div.swiper-slide");
  const mansetNews = mansetSelector.find("a");
  return mansetNews;
}

async function ParseNewsCards(newsCards, co) {
  
  if(typeof(newsCards) === 'undefined')
    return;

  let urls = [];


  co(newsCards).each((index, element) => {
    const anchor = co(element).find("a.img-holder");
    const title = co(anchor).attr("href");
    if(title && !title.includes('http'))
      urls.push(BASE_URL.concat(title));

  })
  return urls;
}

async function ParseMansetNews(mansetNews, co) {
  
  if(typeof(mansetNews) === 'undefined')
    return;

  let urls = [];

  co(mansetNews).each((index, element) => {
    // console.log('index :>> ', index);
    const url = co(element).attr("href");
    urls.push(BASE_URL.concat( url));
  })
  return urls;
}

let numOfErrors = 0;
async function scrapeNews(url) {
    let news = {}
    try {
        // const response = await axios.get(url)
        const response = await fetch(url)
        const data = await response.text();
        // const $ = cheerio.load(response.data)
        const $ = cheerio.load(data)

        const article = $("article");

        const header = article.find("header");
        const title = header.find("h1");
        const summary = header.find("p")
        const ContentMeta = header.find("div.content-meta > div.content-meta-info");
        const agency = ContentMeta.find("div.content-meta-name > span");
        const dateElem = ContentMeta.find("div.content-meta-dates > span");
        const mainImg = article.find("div.main-image > img");
        const articleBodyElem = article.find("div.article-body > p");
        const articleBodyCombined = articleBodyElem.toArray().map(p => $(p).text().trim()).filter(p => p !== '').join('');

        news["title"] = title.text().replace(/\s\(\d+\)/, "").trim();
        news["agency"] = agency.text().replace(/\s\(\d+\)/, "").trim();
        news["date"] = new Date(dateElem.find("time:first").attr("datetime"));
        news["summary"] = summary.text().trim();
        news["mainImage"] = mainImg.attr('src')?.trim();
        news["articleBody"] = articleBodyCombined;

        
    } catch (error) {
        console.log('error :>> ', error);
        numOfErrors++;
    }
    return news;
}

function getUrlChunks(urls, k=4) {
  const size = Math.ceil((urls.length / k));
  const flag = urls.length % k === 0;
  let chunks = [...Array(k).keys()].map(i => urls.slice(size*i, size*(i+1)));
  return chunks;
}

function saveJson(movies) {
  let doc = ""
  for(let i=0; i<movies.length; i++) {
      doc += `{"index":{}}\n`
      doc += JSON.stringify(movies[i]) + "\n"
  }
  fs.writeFile("./movies.json", doc, function(err) { 
      if(err) {
          console.log(err);
      }
  })
}

async function main() {

  const start = Date.now();

  const $ = await DowloadPage();
  const mansetNews = await GetMansetNews($);
  const mansetURLS = await ParseMansetNews(mansetNews, $);
  const newsCards = await GetNewsCards($);
  const newsURLS = await ParseNewsCards(newsCards, $);
  const totalURLS = mansetURLS.concat(newsURLS);

  const chunks = getUrlChunks(totalURLS, 50).filter(item => item.length !== 0)
  let errors = 0;

  // Asynchronously scrape in K "channels"
  const promises = chunks.map(async(chunk) => {
      let result = []
      for(let i=0; i<chunk.length; i++) {
          const movie = await scrapeNews(chunk[i], errors)
          result.push(movie)
      }
      return result
  })

  const records = await Promise.all(promises)
  const movies = [].concat(...records)
  console.log(movies.length)

  const elapsed = (Date.now() - start) / 1000
  console.log(`took ${elapsed} seconds.`)
  console.log(`Number of errors ${numOfErrors}.`)
  saveJson(movies)
}

main();



