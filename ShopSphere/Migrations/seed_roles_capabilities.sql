-- =============================================
-- PostgreSQL Seed Data for Roles and Capabilities
-- ShopSphere API
-- =============================================

-- Begin Transaction
BEGIN;

-- =============================================
-- SEED CAPABILITIES
-- =============================================

-- Product Management Capabilities
INSERT INTO "Capabilities" ("Id", "Name", "Description") VALUES
(1, 'ViewProducts', 'View product listings and details'),
(2, 'CreateProduct', 'Create new products'),
(3, 'UpdateProduct', 'Update existing products'),
(4, 'DeleteProduct', 'Delete products'),
(5, 'ManageProductImages', 'Upload and manage product images');

-- Category Management Capabilities
INSERT INTO "Capabilities" ("Id", "Name", "Description") VALUES
(6, 'ViewCategories', 'View product categories'),
(7, 'CreateCategory', 'Create new categories'),
(8, 'UpdateCategory', 'Update existing categories'),
(9, 'DeleteCategory', 'Delete categories');

-- Order Management Capabilities
INSERT INTO "Capabilities" ("Id", "Name", "Description") VALUES
(10, 'ViewOwnOrders', 'View own order history'),
(11, 'ViewAllOrders', 'View all customer orders'),
(12, 'UpdateOrderStatus', 'Update order status'),
(13, 'CancelOrder', 'Cancel orders'),
(14, 'ProcessRefund', 'Process order refunds');

-- User Management Capabilities
INSERT INTO "Capabilities" ("Id", "Name", "Description") VALUES
(15, 'ViewOwnProfile', 'View own user profile'),
(16, 'UpdateOwnProfile', 'Update own user profile'),
(17, 'ViewAllUsers', 'View all user accounts'),
(18, 'CreateUser', 'Create new user accounts'),
(19, 'UpdateUser', 'Update user accounts'),
(20, 'DeleteUser', 'Delete user accounts'),
(21, 'ManageUserRoles', 'Assign and modify user roles');

-- Cart Management Capabilities
INSERT INTO "Capabilities" ("Id", "Name", "Description") VALUES
(22, 'ViewOwnCart', 'View own shopping cart'),
(23, 'ManageOwnCart', 'Add/remove items from own cart'),
(24, 'Checkout', 'Proceed to checkout');

-- Payment Capabilities
INSERT INTO "Capabilities" ("Id", "Name", "Description") VALUES
(25, 'MakePayment', 'Process payments'),
(26, 'ViewPaymentHistory', 'View own payment history'),
(27, 'ViewAllPayments', 'View all payment transactions'),
(28, 'RefundPayment', 'Refund payments');

-- Reporting Capabilities
INSERT INTO "Capabilities" ("Id", "Name", "Description") VALUES
(29, 'ViewSalesReports', 'View sales analytics and reports'),
(30, 'ViewInventoryReports', 'View inventory reports'),
(31, 'ExportReports', 'Export reports to various formats');

-- System Administration Capabilities
INSERT INTO "Capabilities" ("Id", "Name", "Description") VALUES
(32, 'ManageSystemSettings', 'Configure system settings'),
(33, 'ViewSystemLogs', 'View system logs and audit trails'),
(34, 'ManageRolesCapabilities', 'Create and modify roles and capabilities');

-- =============================================
-- SEED ROLES
-- =============================================

INSERT INTO "Roles" ("Id", "Name", "Description") VALUES
(1, 'Customer', 'Regular customer with basic shopping capabilities'),
(2, 'Admin', 'System administrator with full access');

-- =============================================
-- SEED ROLES-CAPABILITIES RELATIONSHIPS
-- =============================================

-- Customer Role Capabilities (Id: 1) - Standard shopping capabilities
INSERT INTO "RolesCapabilities" ("CapabilitiesId", "RolesId") VALUES
(1, 1),   -- ViewProducts
(6, 1),   -- ViewCategories
(10, 1),  -- ViewOwnOrders
(13, 1),  -- CancelOrder
(15, 1),  -- ViewOwnProfile
(16, 1),  -- UpdateOwnProfile
(22, 1),  -- ViewOwnCart
(23, 1),  -- ManageOwnCart
(24, 1),  -- Checkout
(25, 1),  -- MakePayment
(26, 1);  -- ViewPaymentHistory

-- Admin Role Capabilities (Id: 2) - Full system access
INSERT INTO "RolesCapabilities" ("CapabilitiesId", "RolesId") VALUES
(1, 2),   -- ViewProducts
(2, 2),   -- CreateProduct
(3, 2),   -- UpdateProduct
(4, 2),   -- DeleteProduct
(5, 2),   -- ManageProductImages
(6, 2),   -- ViewCategories
(7, 2),   -- CreateCategory
(8, 2),   -- UpdateCategory
(9, 2),   -- DeleteCategory
(11, 2),  -- ViewAllOrders
(12, 2),  -- UpdateOrderStatus
(13, 2),  -- CancelOrder
(14, 2),  -- ProcessRefund
(17, 2),  -- ViewAllUsers
(18, 2),  -- CreateUser
(19, 2),  -- UpdateUser
(20, 2),  -- DeleteUser
(21, 2),  -- ManageUserRoles
(27, 2),  -- ViewAllPayments
(28, 2),  -- RefundPayment
(29, 2),  -- ViewSalesReports
(30, 2),  -- ViewInventoryReports
(31, 2),  -- ExportReports
(32, 2),  -- ManageSystemSettings
(33, 2),  -- ViewSystemLogs
(34, 2);  -- ManageRolesCapabilities

-- Reset sequences to prevent ID conflicts
SELECT setval(pg_get_serial_sequence('"Capabilities"', 'Id'), 34, true);
SELECT setval(pg_get_serial_sequence('"Roles"', 'Id'), 2, true);

-- Commit Transaction
COMMIT;

-- =============================================
-- Verification Queries (Optional - Comment out if not needed)
-- =============================================
/*
-- Verify Capabilities count
SELECT COUNT(*) AS "Total Capabilities" FROM "Capabilities";

-- Verify Roles count
SELECT COUNT(*) AS "Total Roles" FROM "Roles";

-- Verify Role-Capability mappings
SELECT 
	r."Name" AS "Role",
	COUNT(rc."CapabilitiesId") AS "Capabilities Count"
FROM "Roles" r
LEFT JOIN "RolesCapabilities" rc ON r."Id" = rc."RolesId"
GROUP BY r."Name"
ORDER BY r."Id";

-- View all capabilities for a specific role
SELECT 
	r."Name" AS "Role",
	c."Name" AS "Capability",
	c."Description"
FROM "Roles" r
JOIN "RolesCapabilities" rc ON r."Id" = rc."RolesId"
JOIN "Capabilities" c ON c."Id" = rc."CapabilitiesId"
WHERE r."Name" = 'Customer'
ORDER BY c."Name";
*/
