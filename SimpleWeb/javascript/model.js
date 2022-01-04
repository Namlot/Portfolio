
function cell(cellNum) {
  this.value = cellNum;
  this.scored = false;
  this.showing = false;
}


function GameState() {

  this.clockRunning = false;
  this.clock = 30;
  this.score = 0;
  this.difficulty = "easy"
  this.gameTable = [];
  this.firstPick = -1;
  this.indexFirstPick = 0;
  this.secondPick = -1;
  this.cellArray = [];
  this.gameStarted = false;
  this.gameOver = false;
  this.XMLload = false;

this.resetPicks = function() {
if(this.firstPick.scored == false) {
    this.firstPick.showing = false;
    this.secondPick.showing = false;
  }

  this.firstPick = -1;
  this.secondPick = -1;
}


this.decrementClock = function() {
  if(this.gameOver == false){
  //console.log("decrementing");
//  console.log(this.clock);
  this.clock -= 1;
  if(this.clock <= 0){
    this.gameOver = true;
  }
  else  if((this.difficulty == "Easy" && this.score >= 8) || (this.difficulty == "Medium" && this.score >= 12) || (this.difficulty == "Hard" && this.score >= 15)) {
    this.gameOver = true;
  }
//  console.log(this.clock);
//  console.log("this went through");
 if (this.clock  > 0) {
  setTimeout(this.decrementClock.bind(this), 1000);
  }


}
}


  this.cellRandomizer = function() {
    let gridSize = 0;
    if(this.difficulty == "Easy") {

      gridSize = 16
    }
    if(this.difficulty == "Medium") {
      gridSize = 24;
    }
    if(this.difficulty == "Hard") {
      gridSize = 30;
    }

    for (let i = 0; i < (gridSize/2); i++) {
        this.cellArray[i] = new cell(i);
        }
    for(let i = (gridSize/2); i < gridSize; i++){
        this.cellArray[i] = new cell(i-(gridSize/2));
    }

  };

  this.getCell = function(gridNum) {
    return this.gameTable[gridNum];
  };







  this.ModifystartGame = function(gridSize) {
    if(this.difficulty == "Easy") {
      this.clock = 60;
    }
    if(this.difficulty == "Medium"){
      this.clock = 90;
    }
    if(this.difficulty == "Hard"){
      this.clock = 120;
    }
    if(this.clockRunning == false ){
  this.clockRunning = true;
  this.decrementClock();
  }
    //setInterval(this.decrementClock, 1000)
    this.cellRandomizer();


    var newRequest = new XMLHttpRequest();
                          newRequest.open("GET","gameGrid.json",false);
                         newRequest.send(null);


                          this.cellArray = 	JSON.parse(newRequest.responseText).gameGrid;
                          //console.log(response);






  	this.cellArray = this.arrayRandomizer(this.cellArray);
      for (let i = 0; i < this.cellArray.length; i++) {
        this.gameTable[i] = this.cellArray[i];

       }
    //   console.log(this.gameTable);

  };










this.startGame = function(gridSize) {
  if(this.difficulty == "Easy") {
    this.clock = 60;
  }
  if(this.difficulty == "Medium"){
    this.clock = 90;
  }
  if(this.difficulty == "Hard"){
    this.clock = 120;
  }
  if(this.clockRunning == false ){
this.clockRunning = true;
this.decrementClock();
}
  //setInterval(this.decrementClock, 1000)
  this.cellRandomizer();

if(this.XMLload == true){
  var newRequest = new XMLHttpRequest();
                        newRequest.open("GET","gameGrid.json",false);
                       newRequest.send(null);


                        this.cellArray = 	JSON.parse(newRequest.responseText).gameGrid;
                        //console.log(response);


}



	this.cellArray = this.arrayRandomizer(this.cellArray);
    for (let i = 0; i < this.cellArray.length; i++) {
      this.gameTable[i] = this.cellArray[i];

     }
  //   console.log(this.gameTable);

};
	//array Randomizer
	//precondition fully stocked array is passed
	//postcondition: array will be randomly sorted

this.arrayRandomizer = function(unRandomArray) {

let currentIndex = unRandomArray.length, temporaryValue, randomIndex;


  while (0 !== currentIndex) {


    randomIndex = Math.floor(Math.random() * currentIndex);
    currentIndex -= 1;


    temporaryValue = unRandomArray[currentIndex];
    unRandomArray[currentIndex] = unRandomArray[randomIndex];
    unRandomArray[randomIndex] = temporaryValue;
}
return unRandomArray;

  };

  this.pickCell = function(cellPicked) {
      if(this.getCell(cellPicked).scored == false && this.getCell(cellPicked).showing == false && this.secondPick < 0 ) {
              if(this.firstPick < 0) {

                this.firstPick = this.getCell(cellPicked);
                this.indexFirstPick = cellPicked;
                this.firstPick.showing = true;
                console.log(this.firstPick);
              }
              else if(this.indexFirstPick != cellPicked) {
                this.secondPick = this.getCell(cellPicked);
                this.secondPick.showing = true;
              //  console.log(this.firstPick);
               console.log(this.secondPick);
                if(this.firstPick.value == this.secondPick.value) {
                  this.score += 1;
                  this.firstPick.scored = true;
                  this.secondPick.scored = true;
                  this.resetPicks();
                }

              }
          }
            };

}


var gameMain = new GameState();
