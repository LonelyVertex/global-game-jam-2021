// Utils -----

function pad(str, size) {
    while (str.length < (size || 2)) {str = '0' + str;}
    return str
}

function dec2bin(dec){
    return pad((dec >>> 0).toString(2), 4);
}

// Rendering ----

const maze = document.getElementById('maze')
const cellSize = 50;

function renderTile(tile, top, left) {
    const img = document.createElement('img')
    img.src = `tiles/tile-${tile}.png`
    img.classList.add("tile")
    img.style = `top: ${top * cellSize}px; left: ${left * cellSize}px`
    maze.appendChild(img)
}


// Algorithm ----

// settings
const gridSize = 19
const minTiles = 100
const maxTiles = 150


// constants
const N = 8
const E = 4
const S = 2
const W = 1

const tiles = []
for (i = 1; i < 16; i++) {
    tiles[i - 1] = i
}


// generate
const grid = []
let gridPlaced = 0

for (let i = 0; i < gridSize; i++) {
    grid[i] = []
    for (let j = 0; j < gridSize; j++) {
        grid[i][j] = null
    }
}

const isEndField = (num) => {
    return num === N || num === E || num === S || num === W
}

const addTile = (num, top, left) => {
    grid[top][left] = num
    gridPlaced++
    renderTile(dec2bin(num), top, left)
}

const middle = Math.ceil(gridSize / 2)
const base = 1 + (Math.random() * 4 | 0)
const queue = []


if (base == 1) {
    grid[middle][middle] = N
    renderTile("base-n", middle, middle)
    queue.push([middle - 1, middle])
} else if (base == 2) {
    grid[middle][middle] = W
    renderTile("base-w", middle, middle)
    queue.push([middle, middle - 1])
} else if (base == 3) {
    grid[middle][middle] = S
    renderTile("base-s", middle, middle)
    queue.push([middle + 1, middle])
} else {
    grid[middle][middle] = E
    renderTile("base-e", middle, middle)
    queue.push([middle, middle + 1])
}



while (queue.length > 0) {
    const [top, left] = queue.shift()
    if (grid[top][left] !== null) continue;

    let possibleTiles = tiles.filter(t => true)

    // from top
    if (top > 0 && grid[top - 1][left] !== null) {
        const topTile = grid[top - 1][left];

        if (topTile & S) {
            possibleTiles = possibleTiles.filter(tile => tile & N)
        } else {
            possibleTiles = possibleTiles.filter(tile => !(tile & N))
        }
    } else if (top == 0) {
        possibleTiles = possibleTiles.filter(tile => !(tile & N))
    }

    // from right
    if (left < gridSize - 1 && grid[top][left + 1] !== null) {
        const rightTile = grid[top][left + 1]

        if (rightTile & W) {
            possibleTiles = possibleTiles.filter(tile => tile & E)
        } else {
            possibleTiles = possibleTiles.filter(tile => !(tile & E))
        }
    } else if (left == gridSize - 1) {
        possibleTiles = possibleTiles.filter(tile => !(tile & E))
    }

    // from bot
    if (top < gridSize - 1 && grid[top + 1][left] !== null) {
        const botTile = grid[top + 1][left]

        if (botTile & N) {
            possibleTiles = possibleTiles.filter(tile => tile & S)
        } else {
            possibleTiles = possibleTiles.filter(tile => !(tile & S))
        }
    } else if (top == gridSize - 1) {
        possibleTiles = possibleTiles.filter(tile => !(tile & S))
    }

    // from left
    if (left > 0 && grid[top][left - 1] !== null) {
        const leftTile = grid[top][left - 1];

        if (leftTile & E) {
            possibleTiles = possibleTiles.filter(tile => tile & W)
        } else {
            possibleTiles = possibleTiles.filter(tile => !(tile & W))
        }
    } else if (left == 0) {
        possibleTiles = possibleTiles.filter(tile => !(tile & W))
    }

    if (gridPlaced < minTiles && possibleTiles.some(t => !isEndField(t))) {
        possibleTiles = possibleTiles.filter(tile => !isEndField(tile))
    }
    else if (gridPlaced > maxTiles && possibleTiles.some(isEndField)) {
        possibleTiles = possibleTiles.filter(tile => isEndField(tile))
    }

    const selectedTile = possibleTiles[Math.floor(Math.random() * possibleTiles.length)]

    addTile(selectedTile, top, left);

    if (possibleTiles.length === 0) {
        console.warn("out of tiles")
        break
    }

    if (top + 1 < gridSize && selectedTile & S) queue.push([top + 1, left])
    if (top - 1 >= 0 && selectedTile & N) queue.push([top - 1, left])
    if (left + 1 < gridSize && selectedTile & E) queue.push([top, left + 1])
    if (left - 1 >= 0 && selectedTile & W) queue.push([top, left - 1])
}

for (let i = 0; i < gridSize; i++) {
    for (let j = 0; j < gridSize; j++) {
        if (grid[i][j] === null) {
            addTile(0, i, j)
        }
    }
}
