window.addEventListener('DOMContentLoaded', () => {

    const rangeInput = document.querySelector('#rangeInput')
    const numberInput = document.querySelector('#numberInput')
    const seedInput = document.querySelector('#seed')
    const buttonRandom = document.querySelector('#random')
    const regionElement = document.querySelector('#region')

    let page = 0

    window.addEventListener('scroll', loadNewPageByScroll);
    buttonRandom.addEventListener('click', () => {
        seedInput.value = Math.floor(Math.random() * 100000)
        reloadData()
    })

    function loadNewPageByScroll() {
        if (window.innerHeight + window.scrollY >= document.body.offsetHeight) {
            loadPage(page++);
        }
    }

    function loadPage(page) {
        const seed = getSeed()
        const errors = getErrors()
        const region = getRegion()
        let request = new XMLHttpRequest()
        request.open('GET', `/api/fakeData?seed=${seed}&page=${page}&errors=${errors}&region=${region}`, false)

        request.onload = () => {
            if (request.readyState === request.DONE) {
                if (request.status === 200) {
                    const data = JSON.parse(request.responseText)
                    showDataOnPage(data)
                }
            }
        };

        request.send()
        updateLink()
    }

    function showDataOnPage(data) {
        console.log(data)
        const container = document.querySelector('.table>table>tbody')

        data.forEach((user) => showUserOnPage(user, container))
    }

    function showUserOnPage(user, container) {
        const row = document.createElement('tr')
        for (const data of Object.values(user)) {
            appendCell(data, row)
        }

        container.append(row)
    }

    function appendCell(data, row) {
        const cell = document.createElement('td')
        cell.classList.add('fs-5')
        cell.innerText = data
        row.append(cell)
    }

    (function errorsSync() {
        rangeInput.addEventListener('input', function () {
            numberInput.value = rangeInput.value
            reloadData()
        })

        numberInput.addEventListener('input', function () {
            const errors = Math.min(10, numberInput.value)
            rangeInput.value = errors
            reloadData()
        })
    })()

    seedInput.addEventListener('input', () => {
        reloadData()
    })

    regionElement.addEventListener('change', () => {
        reloadData()
    })


    function getRegion() {
        const selectedIndex = regionElement.selectedIndex
        return regionElement.options[selectedIndex].value
    }

    function getErrors() {
        return numberInput.value || 0
    }

    function getSeed() {
        return seedInput.value || 0
    }

    function reloadData() {
        const container = document.querySelector('.table')
        container.innerHTML = ''
        addTable(container)
        page = 0
        loadPage(page++)
        updateLink()
    }

    function addTable(container) {
        let table = document.createElement('table')
        table.classList.add('table', 'table-striped')
        let head = document.createElement('thead')
        let row = document.createElement('tr');
        ['Index', 'Id', 'Fullname', 'Address', 'Phone'].forEach(item => {
            let header = document.createElement('th')
            header.classList.add('fs-4')
            header.scope = 'col'
            header.innerText = item
            row.append(header)
        })
        head.append(row)
        table.append(head)
        let body = document.createElement('tbody')
        table.append(body)
        container.append(table)
    }
    function updateLink() {
        const button = document.querySelector('#csv')
        const seed = getSeed()
        const errors = getErrors()
        const region = getRegion()
        button.setAttribute("href", `/api/fakeData/download?seed=${seed}&page=${page - 1}&errors=${errors}&region=${region}`)
    }
})