var firstCell;
var secondCell;
var lockout = false;
var deckType;
var timerRunning = false;


function imageSelector(imageNum) {
  var img = document.createElement("img");

  //alert(deckType);
  if(deckType == "animal"){
        if(imageNum == -1) {

          img.src = "gameImages/back.png";
        }
        if(imageNum == 0) {

        img.src = "gameImages/ape.jpg";
        }
        if(imageNum == 1) {

          img.src = "gameImages/cat.jpg";
        }
        if(imageNum == 2) {

          img.src = "gameImages/deer.jpg";
        }
        if(imageNum == 3) {

          img.src = "gameImages/elephant.jpg";
        }
        if(imageNum == 4) {

          img.src = "gameImages/fish.jpg";
        }
        if(imageNum == 5) {

          img.src = "gameImages/parrot.jpg";
        }
        if(imageNum == 6) {

          img.src = "gameImages/peacock.jpg";
        }
        if(imageNum == 7) {

          img.src = "gameImages/snake.jpg";
        }
        if(imageNum == 8) {

          img.src = "gameImages/duck.jpg";
        }
        if(imageNum == 9) {

          img.src = "gameImages/squirell.jpg";
        }
        if(imageNum == 10) {

          img.src = "gameImages/lion.jpg";
        }
        if(imageNum == 11) {

          img.src = "gameImages/hedgehog.jpg";
        }
        if(imageNum == 12) {

          img.src = "gameImages/cow.jpg";
        }
        if(imageNum == 13) {

          img.src = "gameImages/frog.jpg";
        }
        if(imageNum == 14) {

          img.src = "gameImages/crocodile.jpg";
        }
  }
  else {
        if(imageNum == -1) {

          img.src = "gameImages/back.png";
        }
        if(imageNum == 0) {

        img.src = "gameImages/apple.jpg";
        }
        if(imageNum == 1) {

          img.src = "gameImages/avocado.jpg";
        }
        if(imageNum == 2) {

          img.src = "gameImages/banana.jpg";
        }
        if(imageNum == 3) {

          img.src = "gameImages/blueberry.jpg";
        }
        if(imageNum == 4) {

          img.src = "gameImages/cherry.jpg";
        }
        if(imageNum == 5) {

          img.src = "gameImages/dragonfruit.jpg";
        }
        if(imageNum == 6) {

          img.src = "gameImages/grape.jpg";
        }
        if(imageNum == 7) {

          img.src = "gameImages/kiwi.jpg";
        }
        if(imageNum == 8) {

          img.src = "gameImages/lemon.jpg";
        }
        if(imageNum == 9) {

          img.src = "gameImages/orange.jpg";
        }
        if(imageNum == 10) {

          img.src = "gameImages/peach.jpg";
        }
        if(imageNum == 11) {

          img.src = "gameImages/strawberry.jpg";
        }
        if(imageNum == 12) {

          img.src = "gameImages/tomato.jpg";
        }
        if(imageNum == 13) {

          img.src = "gameImages/watermelon.jpg";
        }
        if(imageNum == 14) {

          img.src = "gameImages/pear.jpg";
        }
  }

  return img;
}

function controllerResetModel(){
firstCell.innerHTML = '';
firstCell.appendChild(imageSelector(-1))
secondCell.innerHTML = '';
secondCell.appendChild(imageSelector(-1))
console.log("it came here");
gameMain.resetPicks();
if(gameMain.gameOver == false){
  lockout = false;
  }
}

function CreateGameGrid(difficultySetting) {

  let box = document.querySelector(".box");
  let newElement = document.createElement("a");

  box.appendChild(newElement);

  let tbl = document.createElement("table");
  let counter = 0;
  let indexCounter = 0;

  if (difficultySetting == "Easy") {
      for(let i = 0; i < 4; i++) {
        counter++;
        let newTr = document.createElement("tr");
          for(let j = 0; j < 4; j++) {
            counter++;
            let newTd = document.createElement("td");
            //newTd.innerText = "ImgHere";
            let content = gameMain.getCell(indexCounter);
            newTd.setAttribute("id", indexCounter);
            indexCounter++;
          //  console.log("row: " + i + " " + "column " + j);
          //  console.log(content);

            if(content.showing == true){
              newTd.appendChild(imageSelector(content.value));
            }
            else {
              newTd.appendChild(imageSelector(-1));
            }

            if(counter % 2 == 0) {
              newTd.setAttribute("class", "odd");
            }
            else {
              newTd.setAttribute("class", "even");
            }
            newTr.appendChild(newTd);
          }

          tbl.appendChild(newTr);
        }
      }
      if (difficultySetting == "Medium") {
          for(let i = 0; i < 4; i++) {
            counter++;
            let newTr = document.createElement("tr");
              for(let j = 0; j < 6; j++) {
                counter++;
                let newTd = document.createElement("td");
                //newTd.innerText = "ImgHere";
                let content = gameMain.getCell(indexCounter);
                newTd.setAttribute("id", indexCounter);
                indexCounter++;
              //  console.log("row: " + i + " " + "column " + j);
              //  console.log(content);

                if(content.showing == true){
                  newTd.appendChild(imageSelector(content.value));
                }
                else {
                  newTd.appendChild(imageSelector(-1));
                }

                if(counter % 2 == 0) {
                  newTd.setAttribute("class", "odd");
                }
                else {
                  newTd.setAttribute("class", "even");
                }
                newTr.appendChild(newTd);
              }

              tbl.appendChild(newTr);
            }
          }
          if (difficultySetting == "Hard") {
              for(let i = 0; i < 5; i++) {
                counter++;
                let newTr = document.createElement("tr");
                  for(let j = 0; j < 6; j++) {
                    counter++;
                    let newTd = document.createElement("td");
                    //newTd.innerText = "ImgHere";
                    let content = gameMain.getCell(indexCounter);
                    newTd.setAttribute("id", indexCounter);
                    indexCounter++;
                  //  console.log("row: " + i + " " + "column " + j);
                  //  console.log(content);

                    if(content.showing == true){
                      newTd.appendChild(imageSelector(content.value));
                    }
                    else {
                      newTd.appendChild(imageSelector(-1));
                    }

                    if(counter % 2 == 0) {
                      newTd.setAttribute("class", "odd");
                    }
                    else {
                      newTd.setAttribute("class", "even");
                    }
                    newTr.appendChild(newTd);
                  }

                  tbl.appendChild(newTr);
                }
              }
    box.appendChild(tbl);

    createClickableGrid();

}

function createClickableGrid() {
  var cells = document.getElementsByTagName("td");

  for (var i = 0; i < cells.length; i++) {
    cells[i].onclick = function() {


      if(lockout == false){

	      gameMain.pickCell(this.id);
        if(gameMain.firstPick != -1 && gameMain.secondPick == -1) {
          firstCell = this;
        }
        if(gameMain.secondPick != -1) {
          lockout = true;
          secondCell = this;
          let pickContent = gameMain.getCell(this.id);
          this.innerHTML = '';
          this.appendChild(imageSelector(pickContent.value));



        setTimeout(controllerResetModel, 1000);

        }





        let content = gameMain.getCell(this.id);
    //  console.log(content);
        if(content.showing == true){
      //  content.showing = false;
        this.innerHTML = '';
        this.appendChild(imageSelector(content.value));
  //console.log(this.children[0]);
        }
        else {
        //  content.showing = true;
          this.innerHTML = '';
          this.appendChild(imageSelector(-1));

        }
    }
    changeScoreDiv();
   }
  }


}
function loadInputHandler() {
 gameMain = new GameState();
  gameMain.xmlLoad = true;
  let mybox = document.querySelector(".box");
  mybox.innerHTML = '';
  let gameDifficulty = document.getElementById("selectDifficulty").value;
  //alert(gameDifficulty);
 gameMain.difficulty = gameDifficulty;
 //alert(gameMain.difficulty);
  var playerName = document.getElementById("playerName").value;
//  alert(playerName);
  //gameMain.clockRunning = false;
  //gameMain.gameOver = false;
  //gameMain.score = 0;
  lockout = false;
  deckType = document.getElementById("selectDeck").value;

  gameMain.ModifystartGame();
  CreateGameGrid(gameDifficulty);
  if(timerRunning == false){
    timerRunning = true;
  changeTimerDiv();
}
  var sound = document.createElement("audio");
  sound.src = "music.mp3";
  sound.autoplay = true;
  sound.loop = true;
  mybox.appendChild(sound);
}

function inputHandler() {
  gameMain = new GameState();

  let mybox = document.querySelector(".box");
  mybox.innerHTML = '';
  let gameDifficulty = document.getElementById("selectDifficulty").value;
  //alert(gameDifficulty);
 gameMain.difficulty = gameDifficulty;
 //alert(gameMain.difficulty);
  var playerName = document.getElementById("playerName").value;
//  alert(playerName);
  //gameMain.clockRunning = false;
  //gameMain.gameOver = false;
  //gameMain.score = 0;
  lockout = false;
  deckType = document.getElementById("selectDeck").value;

  gameMain.startGame();
  CreateGameGrid(gameDifficulty);
  if(timerRunning == false){
    timerRunning = true;
  changeTimerDiv();
}
  var sound = document.createElement("audio");
  sound.src = "music.mp3";
  sound.autoplay = true;
  sound.loop = true;
  mybox.appendChild(sound);
}

changeHallDiv = function() {
  if(localStorage.getItem("Easyhighscore") == null){
    localStorage.setItem("Easyhighscore",0);
    localStorage.setItem("EasylowTime",0);
    localStorage.setItem("EasyhighName", "Tayler Tolman");
  }
  if(localStorage.getItem("Mediumhighscore") == null){
    localStorage.setItem("Mediumhighscore",0);
    localStorage.setItem("MediumlowTime",0);
    localStorage.setItem("MediumhighName", "Tayler Tolman");
  }
  if(localStorage.getItem("Hardhighscore") == null){
    localStorage.setItem("Hardhighscore",0);
    localStorage.setItem("HardlowTime",0);
    localStorage.setItem("HardhighName", "Tayler Tolman");
  }
  var hallDiv = document.getElementById("hallOfFame");
  hallDiv.innerHTML = '';
  hallDiv.innerHTML = "Easy: " + localStorage.getItem("EasyhighName") + " " + localStorage.getItem("EasylowTime") + " Seconds Score: " + localStorage.getItem("Easyhighscore");
  hallDiv.innerHTML += "<br>" + "Medium: " + localStorage.getItem("MediumhighName") + " " + localStorage.getItem("MediumlowTime") + " Seconds Score: " + localStorage.getItem("Mediumhighscore");
  hallDiv.innerHTML += "<br>" + "Hard: " + localStorage.getItem("HardhighName") + " " + localStorage.getItem("HardlowTime") + " Seconds Score: " + localStorage.getItem("Hardhighscore");

}


changeTimerDiv = function() {


  var timerDiv = document.getElementById("timerDiv");
  timerDiv.innerHTML = '';
  let currentTime = document.createTextNode(gameMain.clock);
  timerDiv.appendChild(currentTime);
  if(gameMain.gameOver == true){
    gameOver();
  }
  else{
  setTimeout(changeTimerDiv,1000);
 }

}

changeScoreDiv = function() {
  var scoreDiv = document.getElementById("scoreDiv");
  scoreDiv.innerHTML = '';
  let currentScore = document.createTextNode(gameMain.score);
  scoreDiv.appendChild(currentScore);

}


function createInputButton() {
changeHallDiv();
var startButton = document.getElementById("startButton");
startButton.onclick = inputHandler;
var xmlLoad = document.getElementById("loadGame");
xmlLoad.onclick = loadInputHandler;
//var informationDiv = document.getElementById("loginDiv");
//var myText = localStorage.getItem("cs2550timestamp");
//console.log(myText);

//var textNode = document.createTextNode(myText);
//informationDiv.appendChild(textNode);
//console.log(textNode);


var clearButton = document.getElementById("clearButton");
clearButton.onclick = function() {

let mySound = document.getElementsByTagName("audio");
if(mySound[0].muted == false){
  mySound[0].muted = true;
}
else {
  mySound[0].muted = false;
}



};
var localStorageClearButton = document.getElementById("localStorageClear");
localStorageClearButton.onclick = function() {
  console.log("local storage should be cleared now");
  localStorage.clear();
};
}

function gameOver(){
//  let mySound = document.getElementsByTagName("audio");

//  mySound[0].muted = true;
  timerRunning = false;
  lockout = true;
  playerName = document.getElementById("playerName").value;
  if(gameMain.difficulty == "Easy"){

//alert(playerName);
        let Easyhighscore = localStorage.getItem("Easyhighscore");
        let EasylowTime   = localStorage.getItem("EasylowTime");
        let EasyhighName  = localStorage.getItem("EasyhighName");

        if (gameMain.score > Easyhighscore || (gameMain.score >= Easyhighscore && gameMain.clock > EasylowTime)){
          alert("NEW HIGH SCORE!!!");
          localStorage.setItem("Easyhighscore", gameMain.score);
          localStorage.setItem("EasylowTime", gameMain.clock);
          localStorage.setItem("EasyhighName",playerName );
  }
}
if(gameMain.difficulty == "Medium"){


      let Mediumhighscore = localStorage.getItem("Mediumhighscore");
      let MediumlowTime   = localStorage.getItem("MediumlowTime");
      let MediumhighName  = localStorage.getItem("MediumhighName");

      if (gameMain.score > Mediumhighscore || (gameMain.score >= Mediumhighscore && gameMain.clock > MediumlowTime)){
        alert("NEW HIGH SCORE!!!");
        localStorage.setItem("Mediumhighscore", gameMain.score);
        localStorage.setItem("MediumlowTime", gameMain.clock);
        localStorage.setItem("MediumhighName",playerName );
      }
}
if(gameMain.difficulty == "Hard"){


      let Hardhighscore = localStorage.getItem("Hardhighscore");
      let HardlowTime   = localStorage.getItem("HardlowTime");
      let HardhighName  = localStorage.getItem("HardhighName");

      if (gameMain.score > Hardhighscore || (gameMain.score >= Hardhighscore && gameMain.clock > HardlowTime)){
        alert("NEW HIGH SCORE!!!");
        localStorage.setItem("Hardhighscore", gameMain.score);
        localStorage.setItem("HardlowTime", gameMain.clock);
        localStorage.setItem("HardhighName",playerName );
      }
}
changeHallDiv();
  alert("GAMEOVER");
}
