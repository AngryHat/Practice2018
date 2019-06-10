var requestAnimFrame = (function(){
    return window.requestAnimationFrame       ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame    ||
        window.oRequestAnimationFrame      ||
        window.msRequestAnimationFrame     ||
        function(callback){
            window.setTimeout(callback, 1000 / 60);
        };
})();

// Create the canvas
var canvas = document.createElement("canvas");
var ctx = canvas.getContext("2d");
canvas.width = 640;
canvas.height = 480;
document.body.appendChild(canvas);

// main loop
var lastTime;
function main() {
    var now = Date.now();
    var dt = (now - lastTime) / 1000.0;

    update(dt);
    render();

    lastTime = now;
    requestAnimFrame(main);
};

function init() {
    terrainPattern = ctx.createPattern(resources.get('img/terrain_02.png'), 'repeat');

    document.getElementById('play-again').addEventListener('click', function() {
        reset();
    });

    reset();
    lastTime = Date.now();
    main();
}

resources.load([
    'img/sprites_02.png',
    'img/terrain_02.png'
]);
resources.onReady(init);

// game state
var player = {
    pos: [0, 0],
    sprite: new Sprite('img/sprites_02.png', [0, 0], [39, 39], 16, [0, 1])
};

var bullets = [];
var enemies = [];
var explosions = [];

var lastFire = Date.now();
var gameTime = 0;
var isGameOver;
var terrainPattern;

var score = 0;
var scoreEl = document.getElementById('score');

// speed in pixels per second
var playerSpeed = 200;
var bulletSpeed = 500;
var enemySpeed = 100; // original speed is 100px

//megaliths
var megalithsCount = Math.ceil((Math.random() * 4) + 4);
var megaliths = [];

var upMovingBlocked;
var downMovingBlocked;
var rightMovingBlocked;
var leftMovingBlocked;

for(var i = 0; i < megalithsCount; i++)
{
    var randomX = Math.random() * (canvas.width - 120) + 60;
    var randomY = Math.random() * (canvas.height - 60);


    // var megalithsCollisionPrevented = false;
    // while (!megalithsCollisionPrevented)
    // {
    //     if (megaliths.length == 0)
    //     {
    //         megalithsCollisionPrevented = true;
    //     }

    //     for (var item in megaliths){
    //         if (randomX > (item.pos[0] - item.sprite.size[0])
    //             && randomX < item.pos[0] + item.sprite.size[0]){
    //             randomX = Math.random() * (canvas.width - 120) + 60;
    //             break;
    //         }
    //         else if (randomY > item.pos[1] - item.size[1]
    //             && randomY < item.pos[1] + item.sprite.size[1]){
    //             randomY = Math.random() * (canvas.height - 60);
    //             break;
    //         }
    //         else {
    //             megalithsCollisionPrevented = true;
    //         }
    //     }
    // }

    var megalithsCollisionPrevented = false;
    
    while (!megalithsCollisionPrevented)
    {
        if (megaliths.length == 0)
        {
            megalithsCollisionPrevented = true;
        }

        for (var j = 0; j < i; j++){
            if (randomX > (megaliths[j].pos[0] - megaliths[j].sprite.size[0])
                && randomX < megaliths[j].pos[0] + megaliths[j].sprite.size[0]){
                randomX = Math.random() * (canvas.width - 120) + 60;
                break;
            }
            else if (randomY > megaliths[j].pos[1] - megaliths[j].sprite.size[1]
                && randomY < megaliths[j].pos[1] + megaliths[j].sprite.size[1]){
                randomY = Math.random() * (canvas.height - 60);
                break;
            }
        megalithsCollisionPrevented = true;
        }
    }

    if (Math.random() > 0.5)
    {
        megaliths.push({
            pos: 
            [randomX, randomY],
            sprite: new Sprite('img/sprites_02.png', [3, 213], [55, 53], 0, [0], null, true)
        });
    }
    else
    {
        megaliths.push({
            pos: 
            [randomX, randomY],
            sprite: new Sprite('img/sprites_02.png', [5, 273], [48, 43], 0, [0], null, true)
        });
    }
}

// update
function update(dt) {
    gameTime += dt;

    updateEntities(dt);

    // It gets harder over time by adding enemies using this
    // equation: 1-.993^gameTime
    if(Math.random() < 1 - Math.pow(.993, gameTime)) {
        enemies.push({
            pos: [canvas.width,
                  Math.random() * (canvas.height - 39)],
            sprite: new Sprite('img/sprites_02.png', [0, 78], [80, 39],
                               6, [0, 1, 2, 3, 2, 1])
        });
    }

    checkCollisions();

    handleInput(dt);
    
    scoreEl.innerHTML = score;
};

function handleInput(dt) {
    if(input.isDown('DOWN') || input.isDown('s')) {
        // player.pos[1] += playerSpeed * dt;
        if (!downMovingBlocked){
            player.pos[1] += playerSpeed * dt;
        }
    }

    if(input.isDown('UP') || input.isDown('w')) {
        // player.pos[1] -= playerSpeed * dt;
        if (!upMovingBlocked){
            player.pos[1] -= playerSpeed * dt;
        }
    }

    if(input.isDown('LEFT') || input.isDown('a')) {
        // player.pos[0] -= playerSpeed * dt;
        if (!leftMovingBlocked){
        player.pos[0] -= playerSpeed * dt;
        }
    }

    if(input.isDown('RIGHT') || input.isDown('d')) {
        // player.pos[0] += playerSpeed * dt;
        if (!rightMovingBlocked){
            player.pos[0] += playerSpeed * dt;
        }
    }

    if(input.isDown('SPACE') &&
       !isGameOver &&
       Date.now() - lastFire > 100) {
        var x = player.pos[0] + player.sprite.size[0] / 2;
        var y = player.pos[1] + player.sprite.size[1] / 2;

        bullets.push({ pos: [x, y],
                       dir: 'forward',
                       sprite: new Sprite('img/sprites_02.png', [0, 39], [18, 8]) });
        bullets.push({ pos: [x, y],
                       dir: 'up',
                       sprite: new Sprite('img/sprites_02.png', [0, 50], [9, 5]) });
        bullets.push({ pos: [x, y],
                       dir: 'down',
                       sprite: new Sprite('img/sprites_02.png', [0, 60], [9, 5]) });

        lastFire = Date.now();
    }
}

function updateEntities(dt) {
    // update the player sprite animation
    player.sprite.update(dt);

    // update all the bullets
    for(var i=0; i<bullets.length; i++) {
        var bullet = bullets[i];

        switch(bullet.dir) {
        case 'up': bullet.pos[1] -= bulletSpeed * dt; break;
        case 'down': bullet.pos[1] += bulletSpeed * dt; break;
        default:
            bullet.pos[0] += bulletSpeed * dt;
        }

        // remove the bullet if it goes offscreen
        if(bullet.pos[1] < 0 || bullet.pos[1] > canvas.height ||
           bullet.pos[0] > canvas.width) {
            bullets.splice(i, 1);
            i--;
        }
    }

    // update all the enemies
    for(var i=0; i<enemies.length; i++) {
        enemies[i].pos[0] -= enemySpeed * dt;
        enemies[i].sprite.update(dt);

        // remove if offscreen
        if(enemies[i].pos[0] + enemies[i].sprite.size[0] < 0) {
            enemies.splice(i, 1);
            i--;
        }
    }

    // update all the explosions
    for(var i=0; i<explosions.length; i++) {
        explosions[i].sprite.update(dt);

        // remove if animation is done
        if(explosions[i].sprite.done) {
            explosions.splice(i, 1);
            i--;
        }
    }
}

// collisions

function collides(x, y, r, b, x2, y2, r2, b2) {
    return !(r <= x2 || x > r2 ||
             b <= y2 || y > b2);
}

function boxCollides(pos, size, pos2, size2) {
    return collides(pos[0], pos[1],
                    pos[0] + size[0], pos[1] + size[1],
                    pos2[0], pos2[1],
                    pos2[0] + size2[0], pos2[1] + size2[1]);
}




function resetBlockedPaths(){
    downMovingBlocked = false;
    upMovingBlocked = false;
    rightMovingBlocked = false;
    leftMovingBlocked = false;
}

function checkCollisions() {
    checkPlayerBounds();
    resetBlockedPaths();
    
    // collision detection for megaliths and bullets
    for(var i=0; i<megaliths.length; i++) {
        var pos = megaliths[i].pos;
        var size = megaliths[i].sprite.size;

        for(var j=0; j<bullets.length; j++) {
            var pos2 = bullets[j].pos;
            var size2 = bullets[j].sprite.size;

            if(boxCollides(pos, size, pos2, size2)) {
                // remove the bullet and stop this iteration
                bullets.splice(j, 1);
                break;
            }
        }

        //checking blocked paths for player
        if(boxCollides(pos, size, player.pos, player.sprite.size))
        {
            if((player.pos[0] > pos[0] - size[0]) 
                && (player.pos[0] < pos[0]))
            {
                rightMovingBlocked = true;
                // player.pos[0] = pos[0] - player.sprite.size[0]
            }
            else if((player.pos[0] < pos[0] + size[0]) 
            && (player.pos[0] > pos[0]))
            {
                leftMovingBlocked = true;
                // player.pos[0] = pos[0] + size[0]
            }

            if(player.pos[1] > pos[1] - size[1]) 
            {
                upMovingBlocked = true;
                // player.pos[1] = pos[1] - player.sprite.size[1]
            }
            if(player.pos[1] < pos[1] + size[1])
            {
                downMovingBlocked = true;
                // player.pos[1] = pos[1] + player.sprite.size[1]
            }
        }
    }

    //checking collides between enemies and megaliths
    for(var i=0; i<megaliths.length; i++) {
        var pos = megaliths[i].pos;
        var size = megaliths[i].sprite.size;

        for(var j=0; j<enemies.length; j++) {
            var pos2 = enemies[j].pos;
            var size2 = enemies[j].sprite.size;

            if(boxCollides(pos, size, pos2, size2)) {
                if(pos2[0] < pos[0] + size[0] + 10 && pos2[1] > pos[1])
                {
                    pos2[1] += 1;
                }
                else if(pos2[0] < pos[0] + size[0] && pos2[1] < pos[1])
                {
                    pos2[1] -= 1;
                }
            }
        }
    }

    // run collision detection for all enemies and bullets
    for(var i=0; i<enemies.length; i++) {
        var pos = enemies[i].pos;
        var size = enemies[i].sprite.size;

        for(var j=0; j<bullets.length; j++) {
            var pos2 = bullets[j].pos;
            var size2 = bullets[j].sprite.size;

            if(boxCollides(pos, size, pos2, size2)) {
                // Remove the enemy
                enemies.splice(i, 1);
                i--;

                // Add score
                score += 100;

                // Add an explosion
                explosions.push({
                    pos: pos,
                    sprite: new Sprite('img/sprites_02.png',
                                       [0, 117],
                                       [39, 39],
                                       16,
                                       [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
                                       null,
                                       true)
                });

                // Remove the bullet and stop this iteration
                bullets.splice(j, 1);
                break;
            }
        }

        //player enemies collision
        if(boxCollides(pos, size, player.pos, player.sprite.size)) {
            gameOver();
            //game over OFF
        }
    }
}

function checkPlayerBounds() {
    // Check bounds
    if(player.pos[0] < 0) {
        player.pos[0] = 0;
    }
    else if(player.pos[0] > canvas.width - player.sprite.size[0]) {
        player.pos[0] = canvas.width - player.sprite.size[0];
    }

    if(player.pos[1] < 0) {
        player.pos[1] = 0;
    }
    else if(player.pos[1] > canvas.height - player.sprite.size[1]) {
        player.pos[1] = canvas.height - player.sprite.size[1];
    }
}

// draw everything
function render() {
    ctx.fillStyle = terrainPattern;
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    // render the player if the game isn't over
    if(!isGameOver) {
        renderEntity(player);
    }

    renderEntities(bullets);
    renderEntities(enemies);
    renderEntities(explosions);
    renderEntities(megaliths);
    
};

function renderEntities(list) {
    for(var i=0; i<list.length; i++) {
        renderEntity(list[i]);
    }    
}

function renderEntity(entity) {
    ctx.save();
    ctx.translate(entity.pos[0], entity.pos[1]);
    entity.sprite.render(ctx);
    ctx.restore();
}



// game over
function gameOver() {
    document.getElementById('game-over').style.display = 'block';
    document.getElementById('game-over-overlay').style.display = 'block';
    isGameOver = true;
}

// reset game to original state
function reset() {
    document.getElementById('game-over').style.display = 'none';
    document.getElementById('game-over-overlay').style.display = 'none';
    isGameOver = false;
    gameTime = 0;
    score = 0;

    enemies = [];
    bullets = [];

    player.pos = [50, canvas.height / 2];
};
