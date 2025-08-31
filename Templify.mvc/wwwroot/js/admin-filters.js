// Admin Filters Management
class AdminFiltersManager {
    constructor() {
        this.searchInput = document.getElementById('searchInput');
        this.categoryFilter = document.getElementById('categoryFilter');
        this.roleFilter = document.getElementById('roleFilter');
        this.clearFiltersBtn = document.getElementById('clearFilters');
        
        this.init();
    }

    init() {
        // Восстанавливаем фильтры из URL при загрузке страницы
        this.restoreFiltersFromUrl();
        
        // Добавляем обработчики событий
        if (this.searchInput) {
            this.searchInput.addEventListener('input', (e) => {
                this.handleSearch(e.target.value);
                this.saveFiltersToUrl();
            });
        }

        if (this.categoryFilter) {
            this.categoryFilter.addEventListener('change', (e) => {
                this.handleCategoryFilter(e.target.value);
                this.saveFiltersToUrl();
            });
        }

        if (this.roleFilter) {
            this.roleFilter.addEventListener('change', (e) => {
                this.handleRoleFilter(e.target.value);
                this.saveFiltersToUrl();
            });
        }

        if (this.clearFiltersBtn) {
            this.clearFiltersBtn.addEventListener('click', () => {
                this.clearAllFilters();
            });
        }
    }

    handleSearch(searchTerm) {
        const rows = document.querySelectorAll('.admin-table tbody tr');
        
        rows.forEach(row => {
            if (row.classList.contains('no-data')) return;
            
            const text = row.textContent.toLowerCase();
            row.style.display = text.includes(searchTerm.toLowerCase()) ? '' : 'none';
        });
    }

    handleCategoryFilter(category) {
        const rows = document.querySelectorAll('.admin-table tbody tr');
        
        rows.forEach(row => {
            if (row.classList.contains('no-data')) return;
            if (!category) {
                row.style.display = '';
                return;
            }
            
            const badge = row.querySelector('.badge-category');
            if (badge) {
                const badgeText = badge.textContent.toLowerCase().trim();
                const badgeData = badge.getAttribute('data-category');
                
                if (badgeText === category.toLowerCase() || badgeData === category.toLowerCase()) {
                    row.style.display = '';
                } else {
                    row.style.display = 'none';
                }
            } else {
                row.style.display = 'none';
            }
        });
    }

    handleRoleFilter(role) {
        const rows = document.querySelectorAll('.admin-table tbody tr');
        
        rows.forEach(row => {
            if (row.classList.contains('no-data')) return;
            if (!role) {
                row.style.display = '';
                return;
            }
            const badges = row.querySelectorAll('.badge-role');
            let found = false;
            badges.forEach(badge => {
                if (badge.textContent.toLowerCase().includes(role.toLowerCase())) found = true;
            });
            row.style.display = found ? '' : 'none';
        });
    }

    clearAllFilters() {
        if (this.searchInput) this.searchInput.value = '';
        if (this.categoryFilter) this.categoryFilter.value = '';
        if (this.roleFilter) this.roleFilter.value = '';
        
        const rows = document.querySelectorAll('.admin-table tbody tr');
        rows.forEach(row => {
            if (!row.classList.contains('no-data')) {
                row.style.display = '';
            }
        });
        
        this.clearFiltersFromUrl();
    }

    saveFiltersToUrl() {
        const url = new URL(window.location);
        const params = url.searchParams;
        
        if (this.searchInput && this.searchInput.value.trim()) {
            params.set('search', this.searchInput.value.trim());
        } else {
            params.delete('search');
        }
        
        if (this.categoryFilter && this.categoryFilter.value) {
            params.set('category', this.categoryFilter.value);
        } else {
            params.delete('category');
        }
        
        if (this.roleFilter && this.roleFilter.value) {
            params.set('role', this.roleFilter.value);
        } else {
            params.delete('role');
        }
        
        // Обновляем URL без перезагрузки страницы
        window.history.replaceState({}, '', url.toString());
    }

    restoreFiltersFromUrl() {
        const url = new URL(window.location);
        const params = url.searchParams;
        
        const searchValue = params.get('search');
        const categoryValue = params.get('category');
        const roleValue = params.get('role');
        
        // Восстанавливаем поиск
        if (searchValue && this.searchInput) {
            this.searchInput.value = searchValue;
            this.handleSearch(searchValue);
        }
        
        // Восстанавливаем фильтр категории
        if (categoryValue && this.categoryFilter) {
            this.categoryFilter.value = categoryValue;
            this.handleCategoryFilter(categoryValue);
        }
        
        // Восстанавливаем фильтр роли
        if (roleValue && this.roleFilter) {
            this.roleFilter.value = roleValue;
            this.handleRoleFilter(roleValue);
        }
        
        // Если есть значения в полях ввода (из ViewBag), применяем их
        if (this.searchInput && this.searchInput.value.trim()) {
            this.handleSearch(this.searchInput.value);
        }
        
        if (this.categoryFilter && this.categoryFilter.value) {
            this.handleCategoryFilter(this.categoryFilter.value);
        }
        
        if (this.roleFilter && this.roleFilter.value) {
            this.handleRoleFilter(this.roleFilter.value);
        }
    }

    clearFiltersFromUrl() {
        const url = new URL(window.location);
        const params = url.searchParams;
        
        params.delete('search');
        params.delete('category');
        params.delete('role');
        
        window.history.replaceState({}, '', url.toString());
    }
}

// Инициализация при загрузке страницы
document.addEventListener('DOMContentLoaded', function() {
    new AdminFiltersManager();
});
