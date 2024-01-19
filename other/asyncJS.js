async function getData() 
{
    const result = await fetch("https://api.imgflip.com/get_memes");
    const jsonData = await result;
    return jsonData;
}
console.log(getData());

