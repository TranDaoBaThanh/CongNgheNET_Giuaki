// Script for handling todo-specific functionality

// Confirmation modal for delete
function confirmDelete(id, title) {
    const modal = new bootstrap.Modal(document.getElementById('deleteModal'));
    document.getElementById('deleteTaskId').value = id;
    document.getElementById('deleteTaskTitle').textContent = title;
    modal.show();
}

// Enable sorting for the todo list
document.addEventListener('DOMContentLoaded', function () {
    // Setup datepickers if jQuery UI is available
    if (typeof $.fn.datepicker !== 'undefined') {
        $('.datepicker').datepicker({
            format: 'yyyy-mm-dd',
            autoclose: true,
            todayHighlight: true
        });
    }

    // Set up event listeners for priority dropdown changes
    const prioritySelects = document.querySelectorAll('select[name="Priority"]');
    prioritySelects.forEach(select => {
        select.addEventListener('change', function(e) {
            const value = parseInt(e.target.value);
            let colorClass = '';
            
            switch(value) {
                case 1: colorClass = 'text-info'; break;
                case 2: colorClass = 'text-success'; break;
                case 3: colorClass = 'text-primary'; break;
                case 4: colorClass = 'text-warning'; break;
                case 5: colorClass = 'text-danger'; break;
                default: colorClass = 'text-primary';
            }
            
            // Update the label color
            const label = select.closest('.mb-3').querySelector('label');
            if (label) {
                // Remove any existing color classes
                label.className = label.className.replace(/text-\w+/g, '');
                label.classList.add(colorClass);
            }
        });
        
        // Trigger change event to set initial color
        select.dispatchEvent(new Event('change'));
    });

    // Add animation effects for todo items
    const todoItems = document.querySelectorAll('.todo-list .list-group-item');
    todoItems.forEach((item, index) => {
        // Add staggered fade-in effect
        item.style.opacity = '0';
        item.style.transform = 'translateY(20px)';
        item.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
        
        setTimeout(() => {
            item.style.opacity = '1';
            item.style.transform = 'translateY(0)';
        }, index * 100); // Staggered delay based on index
    });
    
    // Add hover effects for action buttons
    const actionButtons = document.querySelectorAll('.todo-actions .btn');
    actionButtons.forEach(button => {
        button.addEventListener('mouseenter', function() {
            this.classList.add('animate__animated', 'animate__pulse');
        });
        
        button.addEventListener('mouseleave', function() {
            this.classList.remove('animate__animated', 'animate__pulse');
        });
    });
});

// Function to handle ajax toggle of task completion (can be implemented if needed)
function toggleTaskStatus(id, element) {
    // Example of how you might implement this with ajax
    // fetch('/Todo/ToggleComplete/' + id, {
    //     method: 'POST',
    //     headers: {
    //         'Content-Type': 'application/json',
    //         'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
    //     }
    // })
    // .then(response => response.json())
    // .then(data => {
    //     if (data.success) {
    //         // Update UI
    //         const todoItem = document.getElementById('todo-' + id);
    //         todoItem.classList.toggle('bg-light');
    //         // Update icon
    //         const icon = element.querySelector('i');
    //         icon.classList.toggle('bi-circle');
    //         icon.classList.toggle('bi-check-circle-fill');
    //     }
    // })
    // .catch(error => console.error('Error:', error));
}
