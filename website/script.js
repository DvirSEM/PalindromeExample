let table = document.getElementsByTagName("table")[0];
let input = document.getElementsByTagName("input")[0];

async function addWord() {
  await fetch("/addWord", {
    method: "POST",
    body: input.value
  });

  getWords();
}

async function getWords() {
  let response = await fetch("/getWords");
  let wordInfos = await response.json();

  input.value = "";
  table.innerHTML = null;

  drawTitle();

  for (let i = 0; i < wordInfos.length; i++) {
    drawWord(wordInfos[i]);
  }
}

function drawTitle() {
  let wordTh = document.createElement("th");
  wordTh.innerText = "Word";
  table.appendChild(wordTh);

  let backwordsTh = document.createElement("th");
  backwordsTh.innerText = "Word Backwards";
  table.appendChild(backwordsTh);

  let isPalindromeTh = document.createElement("th");
  isPalindromeTh.innerText = "Is Palindrome";
  table.appendChild(isPalindromeTh);

  let lengthTh = document.createElement("th");
  lengthTh.innerText = "Character Length";
  table.appendChild(lengthTh);
}

function drawWord(wordInfo) {
  let tr = document.createElement("tr");
  table.appendChild(tr);

  let wordTd = document.createElement("td");
  wordTd.innerText = wordInfo.Word;
  tr.appendChild(wordTd);

  let reverseTd = document.createElement("td");
  reverseTd.innerText = wordInfo.Reversed;
  tr.appendChild(reverseTd);

  let isPalindromeTd = document.createElement("td");
  isPalindromeTd.innerText = wordInfo.IsPalindrome;
  tr.appendChild(isPalindromeTd);

  let lengthTd = document.createElement("td");
  lengthTd.innerText = wordInfo.Length;
  tr.appendChild(lengthTd);
}

getWords();