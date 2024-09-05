document.addEventListener('DOMContentLoaded', function () {
    const params = new URLSearchParams(window.location.search);
    const projectName = params.get('project');
    let currentEditIndex = null;
    if (projectName) {
        document.getElementById('project-name-container').innerHTML = `<h2>Project: ${decodeURIComponent(projectName)}</h2>`;
    }

    // Codemirror editörlerini tanımlayın
    const sentPatternElement = document.getElementById('sent-pattern');
    const receivedPatternElement = document.getElementById('received-pattern');
    let sentPatternEditor, receivedPatternEditor;

    if (sentPatternElement) {
        sentPatternEditor = CodeMirror.fromTextArea(sentPatternElement, {
            lineNumbers: true,
            mode: 'javascript',
        });
    }
    if (receivedPatternElement) {
        receivedPatternEditor = CodeMirror.fromTextArea(receivedPatternElement, {
            lineNumbers: true,
            mode: 'javascript',
        });
    }

    // Home.html sayfasına özgü kodlar

    //OLD
    //const wrapper = document.querySelector('.wrapper');
    //const registerLink = document.querySelector('.register-link');
    //const loginLink = document.querySelector('.login-link');
    //const btnPopup = document.querySelector('.btnLogin-popup');
    //const iconClose = document.querySelector('.icon-close');

    //if (wrapper && registerLink && loginLink && btnPopup && iconClose) {

    //    registerLink.addEventListener('click', () => {
    //        wrapper.classList.add('active');
    //        document.querySelector('.form-box.login').classList.remove('active');
    //        document.querySelector('.form-box.register').classList.add('active');
    //    });

    //    loginLink.addEventListener('click', () => {
    //        wrapper.classList.remove('active');
    //        document.querySelector('.form-box.register').classList.remove('active');
    //        document.querySelector('.form-box.login').classList.add('active');
    //    });

    //    btnPopup.addEventListener('click', () => {
    //        wrapper.style.display = 'flex';
    //        wrapper.classList.add('active-popup');
    //        document.querySelector('.form-box.login').classList.add('active');
    //        document.querySelector('.form-box.register').classList.remove('active');
    //    });

    //    iconClose.addEventListener('click', () => {
    //        wrapper.style.display = 'none';
    //        wrapper.classList.remove('active-popup');
    //        wrapper.classList.remove('active');
    //    });

    //    const registrationForm = document.querySelector('.register-form');
    //    if (registrationForm) {
    //        registrationForm.addEventListener('submit', (event) => {
    //            event.preventDefault();
    //            const fullName = registrationForm.querySelector('input[type="text"]').value;
    //            const email = registrationForm.querySelector('input[type="email"]').value;
    //            const password = registrationForm.querySelector('input[type="password"]').value;
    //            alert(`Registered with Name: ${fullName}, Email: ${email}`);
    //            wrapper.classList.remove('active');
    //        });

    //NEW
    //const wrapper = document.querySelector('.wrapper');
    //const btnPopup = document.querySelector('.btnLogin-popup');
    //const iconClose = document.querySelector('.icon-close');

    //if (wrapper && btnPopup && iconClose) {
    //    btnPopup.addEventListener('click', () => {
    //        wrapper.style.display = 'flex'; // Modal görünür hale getirilir
    //        wrapper.classList.add('active-popup');
    //        document.querySelector('.form-box.login').classList.add('active'); // Login formu aktif hale gelir
    //        if (document.querySelector('.form-box.register')) {
    //            document.querySelector('.form-box.register').classList.remove('active');
    //        }
    //    });

    //    iconClose.addEventListener('click', () => {
    //        wrapper.style.display = 'none'; // Modal kapatılır
    //        wrapper.classList.remove('active-popup');
    //        wrapper.classList.remove('active');
    //    });
    //}

    //// Eğer Register.cshtml sayfasındaysanız, wrapper'ı varsayılan olarak görünür hale getirin
    //if (document.body.classList.contains('register-page')) {
    //    wrapper.style.display = 'flex';
    //}

    //const loginForm = document.querySelector('.login-form');
    //if (loginForm) {
    //    loginForm.addEventListener('submit', (event) => {
    //        event.preventDefault();
    //        const email = loginForm.querySelector('input[type="email"]').value;
    //        const password = loginForm.querySelector('input[type="password"]').value;
    //        alert(`Logged in with Email: ${email}`);
    //        window.location.href = projectsUrl; // Hard-coded URL yerine değişken kullanın
    //    });
    //}
    //abc


    // Projects.html sayfasına özgü kodlar
    const projectList = document.getElementById('project-list');
    const filterInput = document.getElementById('filter');
    

    if (projectList && filterInput ) {
        

        // Filter işlemi
        filterInput.addEventListener('keyup', () => {
            const filterValue = filterInput.value.toLowerCase();
            const projects = Array.from(projectList.getElementsByTagName('li'));
            projects.forEach(project => {
                const projectName = project.querySelector('strong').textContent.toLowerCase();
                project.style.display = projectName.includes(filterValue) ? '' : 'none';
            });
        });

        
    }

   

    // JSON Yönetim sayfasına özgü kodlar
    const jsonContainer = document.getElementById('json-container');
    if (jsonContainer) {
        // JSON verilerini proje adına göre yükleme
        let jsonDataList = loadJsonData(projectName);

        const jsonList = document.getElementById('json-list');
        const searchJsonInput = document.getElementById('search-json');
        const backToProjectsBtn = document.getElementById('back-to-projects-btn');
        const newJsonBtn = document.getElementById('new-json-btn');

      

        newJsonBtn.addEventListener('click', () => {
            clearForm();
            currentEditIndex = null;
        });

        searchJsonInput.addEventListener('input', () => {
            const searchValue = searchJsonInput.value.toLowerCase();
            renderJsonList(jsonDataList.filter(jsonData => jsonData.name.toLowerCase().includes(searchValue)));
        });

        // JSON verilerini kaydetme ve güncelleme
        const saveBtn = document.getElementById('save-btn');
        saveBtn.addEventListener('click', () => {
            const jsonData = {
                name: document.getElementById('json-name').value,
                url: document.getElementById('request-url').value,
                content: document.getElementById('content').value,
                relatedTable: document.getElementById('related-table').value,
                date: document.getElementById('date').value,
                sentPattern: encodeURIComponent(sentPatternEditor ? sentPatternEditor.getValue() : ''), // Encode the sent pattern
                receivedPattern: encodeURIComponent(receivedPatternEditor ? receivedPatternEditor.getValue() : ''), // Encode the received pattern
            };

            if (jsonData.name) {
                if (currentEditIndex !== null) {
                    // Mevcut json verisini güncelle
                    jsonDataList[currentEditIndex] = jsonData;
                    currentEditIndex = null; // Güncelleme sonrası index sıfırla
                } else {
                    // Yeni json verisini ekle
                    jsonDataList.push(jsonData);
                }
                renderJsonList(jsonDataList);
                clearForm();
                saveJsonData(projectName, jsonDataList);
            } else {
                alert('Json Name is required.');
            }
        });

        function renderJsonList(dataList) {
            jsonList.innerHTML = '';
            dataList.forEach((jsonData, index) => {
                const li = document.createElement('li');
                li.innerHTML = `
                     <span>${jsonData.name}</span>
                     <button class="delete-btn" data-index="${index}">Delete</button>
                 `;
                jsonList.appendChild(li);

                li.querySelector('.delete-btn').addEventListener('click', (e) => {
                    e.stopPropagation();
                    if (confirm('Are you sure you want to delete this JSON entry?')) {
                        jsonDataList.splice(index, 1);
                        renderJsonList(jsonDataList);
                        clearForm();
                        saveJsonData(projectName, jsonDataList);
                    }
                });

                li.addEventListener('click', () => {
                    fillForm(jsonData);
                    currentEditIndex = index;
                });
            });
        }

        function fillForm(jsonData) {
            document.getElementById('json-name').value = jsonData.name;
            document.getElementById('request-url').value = jsonData.url;
            document.getElementById('content').value = jsonData.content;
            document.getElementById('related-table').value = jsonData.relatedTable;
            document.getElementById('date').value = jsonData.date;
            if (sentPatternEditor) {
                sentPatternEditor.setValue(decodeURIComponent(jsonData.sentPattern)); // Decode the sent pattern
                sentPatternEditor.refresh();
            }
            if (receivedPatternEditor) {
                receivedPatternEditor.setValue(decodeURIComponent(jsonData.receivedPattern)); // Decode the received pattern
                receivedPatternEditor.refresh();
            }
        }

        function clearForm() {
            document.getElementById('json-name').value = '';
            document.getElementById('request-url').value = '';
            document.getElementById('content').value = '';
            document.getElementById('related-table').value = '';
            document.getElementById('date').value = '';
            if (sentPatternEditor) {
                sentPatternEditor.setValue('');
            }
            if (receivedPatternEditor) {
                receivedPatternEditor.setValue('');
            }
            currentEditIndex = null;
        }

        // JSON verilerini yükleme ve kaydetme işlevleri (proje adına göre)
        function loadJsonData(projectName) {
            const data = localStorage.getItem(`jsonDataList_${projectName}`);
            return data ? JSON.parse(data) : [];
        }

        function saveJsonData(projectName, jsonDataList) {
            localStorage.setItem(`jsonDataList_${projectName}`, JSON.stringify(jsonDataList));
        }

        renderJsonList(jsonDataList);
    }
});



