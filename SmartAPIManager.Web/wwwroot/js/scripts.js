document.addEventListener('DOMContentLoaded', () => {
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
    const wrapper = document.querySelector('.wrapper');
    const registerLink = document.querySelector('.register-link');
    const loginLink = document.querySelector('.login-link');
    const btnPopup = document.querySelector('.btnLogin-popup');
    const iconClose = document.querySelector('.icon-close');

    if (wrapper && registerLink && loginLink && btnPopup && iconClose) {

        registerLink.addEventListener('click', () => {
            wrapper.classList.add('active');
            document.querySelector('.form-box.login').classList.remove('active');
            document.querySelector('.form-box.register').classList.add('active');
        });

        loginLink.addEventListener('click', () => {
            wrapper.classList.remove('active');
            document.querySelector('.form-box.register').classList.remove('active');
            document.querySelector('.form-box.login').classList.add('active');
        });

        btnPopup.addEventListener('click', () => {
            wrapper.style.display = 'flex';
            wrapper.classList.add('active-popup');
            document.querySelector('.form-box.login').classList.add('active');
            document.querySelector('.form-box.register').classList.remove('active');
        });

        iconClose.addEventListener('click', () => {
            wrapper.style.display = 'none';
            wrapper.classList.remove('active-popup');
            wrapper.classList.remove('active');
        });

        const registrationForm = document.querySelector('.register-form');
        if (registrationForm) {
            registrationForm.addEventListener('submit', (event) => {
                event.preventDefault();
                const fullName = registrationForm.querySelector('input[type="text"]').value;
                const email = registrationForm.querySelector('input[type="email"]').value;
                const password = registrationForm.querySelector('input[type="password"]').value;
                alert(`Registered with Name: ${fullName}, Email: ${email}`);
                wrapper.classList.remove('active');
            });
        }

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
    }

    // Projects.html sayfasına özgü kodlar
    const projectList = document.getElementById('project-list');
    const addProjectBtn = document.getElementById('add-project-btn');
    const filterInput = document.getElementById('filter');

    if (projectList && addProjectBtn && filterInput) {
        const modal = document.getElementById('add-project-modal');
        const closeModal = document.querySelector('.close');
        const saveProjectBtn = document.getElementById('save-project-btn');
        const projectNameInput = document.getElementById('project-name');
        const projectDescriptionInput = document.getElementById('project-description');
        const projectFileInput = document.getElementById('project-file');
        const projectDateInput = document.getElementById('project-date');

        const projectDetailsModal = document.getElementById('project-details-modal');
        const closeDetailsModal = projectDetailsModal.querySelector('.close');
        const detailProjectName = document.getElementById('detail-project-name');
        const detailProjectDescription = document.getElementById('detail-project-description');
        const detailProjectDate = document.getElementById('detail-project-date');
        const detailProjectFile = document.getElementById('detail-project-file');

        const editModal = document.getElementById('edit-project-modal');
        const closeEditModal = document.querySelector('.close-edit');
        const updateProjectBtn = document.getElementById('update-project-btn');
        const editProjectNameInput = document.getElementById('edit-project-name');
        const editProjectDescriptionInput = document.getElementById('edit-project-description');
        const editProjectFileInput = document.getElementById('edit-project-file');
        const editProjectDateInput = document.getElementById('edit-project-date');

        let projects = [];
        let currentEditIndex = null;

        addProjectBtn.addEventListener('click', () => {
            modal.style.display = 'flex';
        });

        closeModal.addEventListener('click', () => {
            modal.style.display = 'none';
        });

        closeDetailsModal.addEventListener('click', () => {
            projectDetailsModal.style.display = 'none';
        });

        closeEditModal.addEventListener('click', () => {
            editModal.style.display = 'none';
        });

        saveProjectBtn.addEventListener('click', () => {
            const projectName = projectNameInput.value.trim();
            const projectDescription = projectDescriptionInput.value.trim();
            const projectFile = projectFileInput.files[0] ? projectFileInput.files[0].name : '';
            const projectDate = projectDateInput.value;

            if (projectName) {
                const project = {
                    name: projectName,
                    description: projectDescription,
                    file: projectFile,
                    date: projectDate
                };
                projects.push(project);
                renderProjects();
                modal.style.display = 'none';
                clearInputs();
            } else {
                alert('Please enter the project name');
            }
        });

        updateProjectBtn.addEventListener('click', () => {
            const projectName = editProjectNameInput.value.trim();
            const projectDescription = editProjectDescriptionInput.value.trim();
            const projectFile = editProjectFileInput.files[0] ? editProjectFileInput.files[0].name : projects[currentEditIndex].file;
            const projectDate = editProjectDateInput.value;

            if (projectName) {
                projects[currentEditIndex] = {
                    name: projectName,
                    description: projectDescription,
                    file: projectFile,
                    date: projectDate
                };

                renderProjects();
                editModal.style.display = 'none';
            } else {
                alert('Please fill in the Project Name field');
            }
        });

        function renderProjects() {
            projectList.innerHTML = '';
            projects.forEach((project, index) => {
                const li = document.createElement('li');
                li.innerHTML = `
                    <strong>${project.name}</strong>
                    <p>${project.description}</p>
                    <p>${project.date}</p>
                    <button class="edit-btn" data-index="${index}">Edit</button>
                    <button class="delete-btn" data-index="${index}">Delete</button>
                `;
                projectList.appendChild(li);

                // Proje öğesine tıklama işlevi
                li.addEventListener('click', () => {
                    window.location.href = `json.html?project=${encodeURIComponent(project.name)}`;
                });
            });

            document.querySelectorAll('.edit-btn').forEach(button => {
                button.addEventListener('click', (e) => {
                    e.stopPropagation();
                    currentEditIndex = e.target.dataset.index;
                    const project = projects[currentEditIndex];
                    editProjectNameInput.value = project.name;
                    editProjectDescriptionInput.value = project.description;
                    editProjectDateInput.value = project.date;
                    editModal.style.display = 'flex';
                });
            });

            document.querySelectorAll('.delete-btn').forEach(button => {
                button.addEventListener('click', (e) => {
                    e.stopPropagation();
                    const index = e.target.dataset.index;
                    if (confirm('Are you sure you want to delete this project?')) {
                        projects.splice(index, 1);
                        renderProjects();
                    }
                });
            });
        }

        function clearInputs() {
            projectNameInput.value = '';
            projectDescriptionInput.value = '';
            projectFileInput.value = '';
            projectDateInput.value = '';
        }

        filterInput.addEventListener('keyup', () => {
            const filterValue = filterInput.value.toLowerCase();
            const filteredProjects = projects.filter(project => project.name.toLowerCase().includes(filterValue));
            projectList.innerHTML = '';
            filteredProjects.forEach((project, index) => {
                const li = document.createElement('li');
                li.innerHTML = `
                    <strong>${project.name}</strong>
                    <p>${project.description}</p>
                    <p>${project.date}</p>
                    <button class="edit-btn" data-index="${index}">Edit</button>
                    <button class="delete-btn" data-index="${index}">Delete</button>
                `;
                projectList.appendChild(li);
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

        //if (backToProjectsBtn) {
        //    backToProjectsBtn.addEventListener('click', () => {
        //        window.location.href = projectsPageUrl; 
        //    });
        //}

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
